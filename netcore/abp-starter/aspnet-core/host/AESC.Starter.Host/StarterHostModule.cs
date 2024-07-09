using AESC.Sample;
using AESC.Shared;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace AESC.Starter.Host;

[DependsOn(
    typeof(AbpAccountWebModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpProSharedHostingMicroserviceModule),
    typeof(SharedAppModule),
    typeof(SampleAppModule)
)]
public class StarterHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = true; });

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseMySQL();
        });

        context.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        var configuration = context.Services.GetConfiguration();
        ConfigureCache(context);
        ConfigureSwaggerServices(context);
        ConfigureJwtAuthentication(context, configuration);
        ConfigureMiniProfiler(context);
        ConfigureIdentity(context);
        ConfigureAuditLog(context);
        ConfigurationSignalR(context);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        app.UseAbpProRequestLocalization();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseMiniProfiler();
        app.UseRouting();
        app.UseCors(StarterHostConst.DefaultCorsPolicyName);
        app.UseAuthentication();

        app.UseMultiTenancy();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/Starter/swagger.json", "Starter API");
            options.DocExpansion(DocExpansion.None);
            options.DefaultModelsExpandDepth(-1);
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();

        app.UseUnitOfWork();
        app.UseConfiguredEndpoints(endpoints => { endpoints.MapHealthChecks("/health"); });

        AsyncHelper.RunSync(async () =>
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            }
        });
    }

    /// <summary>
    /// Redis缓存
    /// </summary>
    private void ConfigureCache(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedCacheOptions>(options =>
        {
            options.KeyPrefix = "Starter:";
        });

        var configuration = context.Services.GetConfiguration();
        var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
        context.Services
            .AddDataProtection()
            .PersistKeysToStackExchangeRedis(redis, "Starter-Protection-Keys");
    }

    private static void ConfigureSwaggerServices(ServiceConfigurationContext context)
    {
        context.Services.AddSwaggerGen(options =>
        {
            // 文件下载类型
            options.MapType<FileContentResult>(() => new OpenApiSchema() { Type = "file" });

            options.SwaggerDoc("Starter", new OpenApiInfo { Title = "AESCStarter API", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.EnableAnnotations(); // 启用注解
            options.DocumentFilter<HiddenAbpDefaultApiFilter>();
            options.SchemaFilter<EnumSchemaFilter>();
            // 加载所有xml注释，这里会导致swagger加载有点缓慢
            var xmlPaths = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var xml in xmlPaths)
            {
                options.IncludeXmlComments(xml, true);
            }

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme()
                {
                    Description = "直接在下框输入JWT生成的Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme, Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });

            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "Accept-Language",
                Description = "多语言设置，系统预设语言有zh-Hans、en，默认为zh-Hans",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                            { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    /// <summary>
    /// 配置JWT
    /// </summary>
    private void ConfigureJwtAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                // 是否开启签名认证
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                //ClockSkew = TimeSpan.Zero,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(configuration["Jwt:SecurityKey"]))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = currentContext =>
                {
                    var path = currentContext.HttpContext.Request.Path;
                    if (path.StartsWithSegments("/login"))
                    {
                        return Task.CompletedTask;
                    }

                    var accessToken = string.Empty;
                    if (currentContext.HttpContext.Request.Headers.ContainsKey("Authorization"))
                    {
                        accessToken = currentContext.HttpContext.Request.Headers["Authorization"];
                        if (!string.IsNullOrWhiteSpace(accessToken))
                        {
                            accessToken = accessToken.Split(" ").LastOrDefault();
                        }
                    }

                    if (accessToken.IsNullOrWhiteSpace())
                    {
                        accessToken = currentContext.Request.Query["access_token"].FirstOrDefault();
                    }

                    if (accessToken.IsNullOrWhiteSpace())
                    {
                        accessToken = currentContext.Request.Cookies[StarterHostConst.DefaultCookieName];
                    }

                    currentContext.Token = accessToken;
                    currentContext.Request.Headers.Remove("Authorization");
                    currentContext.Request.Headers.Add("Authorization", $"Bearer {accessToken}");

                    return Task.CompletedTask;
                }
            };
        });
    }

    /// <summary>
    /// 配置MiniProfiler
    /// </summary>
    private void ConfigureMiniProfiler(ServiceConfigurationContext context)
    {
        context.Services.AddMiniProfiler(options => options.RouteBasePath = "/profiler").AddEntityFramework();
    }

    /// <summary>
    /// 配置Identity
    /// </summary>
    private void ConfigureIdentity(ServiceConfigurationContext context)
    {
        context.Services.Configure<IdentityOptions>(options => { options.Lockout = new LockoutOptions() { AllowedForNewUsers = false }; });
    }

    /// <summary>
    /// 审计日志
    /// </summary>
    private void ConfigureAuditLog(ServiceConfigurationContext context)
    {
        Configure<AbpAuditingOptions>
        (
            options =>
            {
                options.IsEnabled = true;
                options.EntityHistorySelectors.AddAllEntities();
                options.ApplicationName = "AESC.Starter";
            }
        );

        Configure<AbpAspNetCoreAuditingOptions>(
            options =>
            {
                options.IgnoredUrls.Add("/AuditLogs/page");
                options.IgnoredUrls.Add("/hangfire/stats");
                options.IgnoredUrls.Add("/cap");
            });
    }

    private void ConfigurationSignalR(ServiceConfigurationContext context)
    {
        var redisConnection = context.Services.GetConfiguration().GetValue<string>("Redis:Configuration");

        if (redisConnection.IsNullOrWhiteSpace())
        {
            throw new UserFriendlyException(message: "Redis连接字符串未配置.");
        }

        context.Services.AddSignalR().AddStackExchangeRedis(redisConnection, options => { options.Configuration.ChannelPrefix = "Lion.AbpPro"; });
    }
}