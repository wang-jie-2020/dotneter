using System.Globalization;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Auditing;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;
using Yi.Admin;
using Yi.AspNetCore;
using Yi.AspNetCore.Extensions;
using Yi.AspNetCore.SqlSugarCore;
using Yi.AspNetCore.System.Permissions;
using Yi.System;
using Yi.System.Options;

namespace Yi.Web;

[DependsOn(
    typeof(YiAspNetCoreModule),
    typeof(YiAdminModule),
    typeof(YiSystemModule)
)]
public class YiAbpWebModule : AbpModule
{
    private const string DefaultCorsPolicyName = "Default";

    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var host = context.Services.GetHostingEnvironment();

        context.Services.AddYiDbContext<YiDbContext>();
        context.Services.AddTransient(x => x.GetRequiredService<ISqlSugarDbContext>().SqlSugarClient);

        //请求日志
        Configure<AbpAuditingOptions>(options =>
        {
            //默认关闭，开启会有大量的审计日志
            options.IsEnabled = false;
            //审计日志过滤器
            options.AlwaysLogSelectors.Add(x => Task.FromResult(true));
        });

        //设置api格式
        context.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

        //设置缓存不要过期，默认滑动20分钟
        Configure<AbpDistributedCacheOptions>(cacheOptions =>
        {
            cacheOptions.GlobalCacheEntryOptions.SlidingExpiration = null;
            //缓存key前缀
            cacheOptions.KeyPrefix = "Yi:";
        });

        Configure<AbpAntiForgeryOptions>(options => { options.AutoValidate = false; });

        //Swagger
        context.Services.AddYiSwaggerGen<YiAbpWebModule>(options => { options.SwaggerDoc("default", new OpenApiInfo { Title = "Yi", Version = "v1", Description = "Yi" }); });

        //跨域
        context.Services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicyName, builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]!
                            .Split(";", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray()
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        //配置多租户
        Configure<AbpTenantResolveOptions>(options =>
        {
            //基于cookie jwt不好用，有坑
            options.TenantResolvers.Clear();
            options.TenantResolvers.Add(new HeaderTenantResolveContributor());
            //options.TenantResolvers.Add(new HeaderTenantResolveContributor());
            //options.TenantResolvers.Add(new CookieTenantResolveContributor());

            //options.TenantResolvers.RemoveAll(x => x.Name == CookieTenantResolveContributor.ContributorName);
        });

        //速率限制
        //每60秒限制100个请求，滑块添加，分6段
        context.Services.AddRateLimiter(_ =>
        {
            _.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            _.OnRejected = (context, _) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");

                return new ValueTask();
            };

            //全局使用，链式表达式
            _.GlobalLimiter = PartitionedRateLimiter.CreateChained(
                PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    var userAgent = httpContext.Request.Headers.UserAgent.ToString();

                    return RateLimitPartition.GetSlidingWindowLimiter
                    (userAgent, _ =>
                        new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 1000,
                            Window = TimeSpan.FromSeconds(60),
                            SegmentsPerWindow = 6,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                }));
        });

        //jwt鉴权
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        var refreshJwtOptions = configuration.GetSection(nameof(RefreshJwtOptions)).Get<RefreshJwtOptions>();

        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken)) context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            })
            .AddJwtBearer(TokenClaimConst.Refresh, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = refreshJwtOptions.Issuer,
                    ValidAudience = refreshJwtOptions.Audience,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshJwtOptions.SecurityKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var refresh_token = context.Request.Headers["refresh_token"];
                        if (!string.IsNullOrEmpty(refresh_token))
                        {
                            context.Token = refresh_token;
                            return Task.CompletedTask;
                        }

                        var refreshToken = context.Request.Query["refresh_token"];
                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            context.Token = refreshToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        //授权
        context.Services.AddAuthorization();

        return Task.CompletedTask;
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var service = context.ServiceProvider;

        var env = context.GetEnvironment();
        var app = context.GetApplicationBuilder();

        app.UseRouting();

        //跨域
        app.UseCors(DefaultCorsPolicyName);

        //速率限制
        app.UseRateLimiter();

        //无感token，先刷新再鉴权
        //app.UseRefreshToken();

        //鉴权
        app.UseAuthentication();

        //多租户
        app.UseMultiTenancy();

        //swagger
        app.UseYiSwagger(c => c.SwaggerEndpoint("/swagger/default/swagger.json", "default"));

        //静态资源
        app.UseStaticFiles();

        //工作单元
        app.UseUnitOfWork();

        //授权
        app.UseAuthorization();

        //审计日志
        app.UseAuditing();

        //日志记录
        app.UseAbpSerilogEnrichers();

        //终节点
        app.UseConfiguredEndpoints();

        return Task.CompletedTask;
    }
}