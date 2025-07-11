using System.Globalization;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.IdentityModel.Tokens;
using Yi.AspNetCore.Data.Seeding;
using Yi.AspNetCore.Extensions.Builder;
using Yi.AspNetCore.Extensions.DependencyInjection;
using Yi.AspNetCore.Security;
using Yi.Framework.Extensions.Builder;
using Yi.Framework.Options;
using Yi.System;

namespace Yi.Admin;

[DependsOn(typeof(SystemModule))]
public class AdminModule : AbpModule
{
    private const string DefaultCorsPolicyName = "Default";

    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var host = context.Services.GetHostingEnvironment();

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
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        //速率限制 每60秒限制100个请求，滑块添加，分6段
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
                    // OnMessageReceived = context =>
                    // {
                    //     var accessToken = context.Request.Query["access_token"];
                    //     if (!string.IsNullOrEmpty(accessToken)) context.Token = accessToken;
                    //
                    //     return Task.CompletedTask;
                    // }
                    // OnForbidden = async context =>
                    // {
                    //     await context.HttpContext.Response.WriteAsync("forbidden");
                    // }
                };
            })
            .AddJwtBearer(ClaimsIdentityTypes.Refresh, options =>
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

                        // var refreshToken = context.Request.Query["refresh_token"];
                        // if (!string.IsNullOrEmpty(refreshToken))
                        // {
                        //     context.Token = refreshToken;
                        // }

                        return Task.CompletedTask;
                    }
                };
            });

        //授权
        context.Services.AddAuthorization();

        //Swagger
        context.Services.AddYiSwaggerGen<AdminModule>();

        //miniProfiler
        context.Services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
        });

        // Db
        context.Services.AddTransient<ISqlSugarDbContext, AdminDbContext>();

        return Task.CompletedTask;
    }

    public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var service = context.ServiceProvider;

        //尝试创建数据库
        //var moduleContainer = service.GetRequiredService<IModuleContainer>();
        //var db = service.GetRequiredService<ISqlSugarDbContext>().SqlSugarClient;
        //db.DbMaintenance.CreateDatabase();

        //var types = new List<Type>();
        //foreach (var module in moduleContainer.Modules)
        //    types.AddRange(module.Assembly.GetTypes()
        //        .Where(x => x.GetCustomAttribute<IgnoreCodeFirstAttribute>() == null)
        //        .Where(x => x.GetCustomAttribute<SugarTable>() != null)
        //        .Where(x => x.GetCustomAttribute<SplitTableAttribute>() is null));
        //if (types.Count > 0) db.CopyNew().CodeFirst.InitTables(types.ToArray());

        //尝试种子数据
        var dataSeeder = service.GetRequiredService<IDataSeeder>();
        await dataSeeder.SeedAsync();
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
        app.UseYiSwagger(c =>
        {
            c.InjectJavascript("/swagger/ui/Customization.js");
        });

        //MiniProfiler
        app.UseMiniProfiler();

        //静态资源
        app.UseStaticFiles();

        app.UseRequestLocalization(options =>
        {
            var defaultCulture = new CultureInfo("zh-CN");
            defaultCulture.DateTimeFormat.SetAllDateTimePatterns(new[] { "H:mm:ss" }, 'T');
            defaultCulture.DateTimeFormat.SetAllDateTimePatterns(new[] { "H:mm" }, 't');

            options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            options.SupportedCultures = options.SupportedUICultures = new List<CultureInfo>
            {
                new("en"),
                new("fr"),
                new("zh-CN"),
            };
        });

        //授权
        app.UseAuthorization();

        //审计日志
        app.UseAuditing();

        //终节点
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return Task.CompletedTask;
    }
}