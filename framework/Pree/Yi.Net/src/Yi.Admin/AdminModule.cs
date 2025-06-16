using System.Globalization;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Yi.AspNetCore;
using Yi.AspNetCore.Core.Permissions;
using Yi.AspNetCore.Extensions;
using Yi.AspNetCore.MultiTenancy;
using Yi.System;
using Yi.System.Options;

namespace Yi.Web;

[DependsOn(
    typeof(YiAspNetCoreModule),
    typeof(SystemModule)
)]
public class AdminModule : AbpModule
{
    private const string DefaultCorsPolicyName = "Default";

    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var host = context.Services.GetHostingEnvironment();

        context.Services.AddYiDbContext<AdminDbContext>();
        context.Services.AddTransient(x => x.GetRequiredService<ISqlSugarDbContext>().SqlSugarClient);


        //Configure<AbpLocalizationOptions>(options =>
        //{
        //    options.Resources.Get<DefaultResource>()
        //        .AddVirtualJson("/Resources");
        //});

        // Configure<AbpExceptionHandlingOptions>(options =>
        // {
        //     options.SendExceptionsDetailsToClients = host.IsDevelopment() || configuration["App:SendExceptions"] == "true";
        // });

        //设置api格式
        context.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

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

        //Swagger
        context.Services.AddYiSwaggerGen<AdminModule>(options =>
        {
            options.SwaggerDoc("default", new OpenApiInfo { Title = "Yi", Version = "v1", Description = "Yi" });
        });

        //miniProfiler
        context.Services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
        });

        //minio
        //if (configuration["Minio:IsEnabled"].To<bool>())
        //{
        //    context.Services.UseMinio(options =>
        //    {
        //        options.Containers.ConfigureAll((containerName, containerConfiguration) =>
        //        {
        //            containerConfiguration.Bucket(bucket =>
        //            {
        //                bucket.EndPoint = configuration["Minio:Default:EndPoint"];
        //                bucket.AccessKey = configuration["Minio:Default:User"];
        //                bucket.SecretKey = configuration["Minio:Default:Pwd"];
        //                bucket.BucketName = configuration["Minio:Default:BucketName"];
        //                bucket.WithSSL = configuration["Minio:Default:WithSSL"].To<bool>();
        //            });
        //        });
        //    });
        //}

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
        app.UseYiSwagger(c =>
        {
            c.InjectJavascript("/swagger/ui/Customization.js");
            c.SwaggerEndpoint("/swagger/default/swagger.json", "default");
        });

        //MiniProfiler
        app.UseMiniProfiler();
        
        //静态资源
        app.UseStaticFiles();

        //todo
        //app.UseAbpRequestLocalization(options =>
        //{
        //    var defaultCulture = new CultureInfo("zh-CN");
        //    defaultCulture.DateTimeFormat.SetAllDateTimePatterns(new[] { "H:mm:ss" }, 'T');
        //    defaultCulture.DateTimeFormat.SetAllDateTimePatterns(new[] { "H:mm" }, 't');

        //    options.DefaultRequestCulture = new RequestCulture(defaultCulture);

        //    options.SupportedCultures = options.SupportedUICultures = new List<CultureInfo>
        //    {
        //        new("en"),
        //        new("fr"),
        //        new("zh-CN"),
        //        new("zh-Hans")
        //    };
        //});

        //授权
        app.UseAuthorization();

        //审计日志
        app.UseAuditing();

        //终节点
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            endpoints.MapRazorPages();

            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    var options = app.ApplicationServices
            //        .GetRequiredService<IOptions<AbpEndpointRouterOptions>>()
            //        .Value;
            //    if (options.EndpointConfigureActions.Any())
            //    {
            //        var endpointRouteBuilderContext = new EndpointRouteBuilderContext(endpoints, scope.ServiceProvider);
            //        foreach (var configureAction in options.EndpointConfigureActions)
            //        {
            //            configureAction(endpointRouteBuilderContext);
            //        }
            //    }
            //}
        });
        
        return Task.CompletedTask;
    }
}