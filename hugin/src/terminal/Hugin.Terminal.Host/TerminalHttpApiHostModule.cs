using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Hangfire;
using Hangfire.MySql;
using Hugin.Cache.CsRedis;
using Hugin.Infrastructure.Cap;
using Hugin.Mvc;
using Hugin.Platform;
using Hugin.Platform.EntityFrameworkCore;
using Hugin.Platform.Localization;
using Hugin.Terminal.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Hugin.Terminal.Localization;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Http.Client.IdentityModel.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Threading;

namespace Hugin.Terminal
{
    [DependsOn(
        //引入服务
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpSwashbuckleModule),
        typeof(CsRedisCacheModule),
        typeof(HuginMvcModule),
        //引入模块
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreMvcClientModule),
        typeof(AbpHttpClientIdentityModelWebModule),
        //typeof(WechatSenparcHttpApiModule),
        //引入项目
        typeof(TerminalApplicationModule),
        typeof(TerminalHttpApiModule),
        typeof(PlatformEntityFrameworkCoreModule)
    )]
    public class TerminalHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        private static readonly ApiInfo[] HostApiGroup = new[] { ApiGroups.AbpGroup, ApiGroups.TerminalGroup };

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            #region Persistent

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL(opt =>
                {
                    opt.UseQuerySplittingBehavior(Microsoft.EntityFrameworkCore.QuerySplittingBehavior.SingleQuery);
                });
            });

            Configure<CsRedisCacheOptions>(cacheOptions =>
            {
                var redisConfiguration = configuration["Redis:Terminal"];
                if (!redisConfiguration.IsNullOrEmpty())
                {
                    cacheOptions.ConnectionString = redisConfiguration;
                }
            });

            context.Services.AddHangfire(options =>
            {
                options.UseStorage(new MySqlStorage(
                    configuration.GetConnectionString(PlatformConsts.DbProperties.ConnectionStringName),
                    new MySqlStorageOptions
                    {
                        TablesPrefix = "hangfire."
                    }));
            });

            if (configuration["App:HangFire:EnabledServer"] == "true")
            {
                context.Services.AddHangfireServer(options =>
                {
#if DEBUG
                    options.Queues = new[] { "debug" };
#else
                    options.Queues = new[] { "default" };
#endif
                });
            }

            context.Services.AddCap(options =>
            {
                options.UseMySql(configuration.GetConnectionString(PlatformConsts.DbProperties.ConnectionStringName));
                options.UseRabbitMQ(config =>
                {
                    config.HostName = configuration["RabbitMQ:HostName"];
                    config.UserName = configuration["RabbitMQ:UserName"];
                    config.Password = configuration["RabbitMQ:Password"];
                });
                options.UseDashboard();
            });

            if (configuration["App:Cap:EnabledServer"] == "false")
            {
                context.Services.Replace(ServiceDescriptor.Singleton<IConsumerServiceSelector, NullConsumerServiceSelector>());
            }

            #endregion

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = TerminalConsts.Name + ":";
            });

            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = configuration["AuthServer:Audience"];
                });

            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers
                    .Create(typeof(TerminalApplicationModule).Assembly, opt =>
                    {
                        opt.RootPath = TerminalConsts.NameLower;
                    });
            });

            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
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

            Configure<AbpLocalizationOptions>(options =>
            {
                options.DefaultResourceType = typeof(TerminalResource);
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });

            context.Services.Replace(ServiceDescriptor.Transient<ILanguageProvider, DefaultLanguageProvider>());
            context.Services.Replace(ServiceDescriptor.Transient<ISettingProvider, SettingProvider>());
            Configure<AbpLocalizationOptions>(options =>
            {
                if (options.GlobalContributors.Contains<RemoteLocalizationContributor>())
                {
                    options.GlobalContributors.Remove<RemoteLocalizationContributor>();
                }
            });

            ConfigureDevelopmentServices(context);
        }

        protected virtual void ConfigureDevelopmentServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            if (hostingEnvironment.IsProduction())
            {
                return;
            }

            context.Services.AddAbpSwaggerGenWithOAuth(configuration["AuthServer:Authority"],
                    scopes: new Dictionary<string, string>
                    {
                        {"Terminal", "终端内容管理"}    //todo 
                    },
                    setupAction: options =>
                    {
                        Array.ForEach(HostApiGroup, p =>
                         {
                             options.SwaggerDoc(p.Name, p.OpenApiInfo);
                         });

                        options.DocInclusionPredicate((docName, apiDesc) =>
                        {
                            var groupName = apiDesc.GroupName ?? string.Empty;
                            if (HostApiGroup.Select(p => p.Name).Contains(groupName))
                            {
                                return docName == groupName;
                            }

                            return docName == ApiGroups.Abp;
                        });

                        options.EnableAnnotations();
                        options.CustomSchemaIds(type => type.FullName);
                        options.DescribeAllParametersInCamelCase();
                        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                                typeof(TerminalApplicationModule).Assembly.GetName().Name + ".xml"));
                        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                                typeof(TerminalApplicationContractsModule).Assembly.GetName().Name + ".xml"));
                        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                                typeof(TerminalHttpApiModule).Assembly.GetName().Name + ".xml"));
                    });

            context.Services.AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; }).AddEntityFramework();

#if DEBUG
            context.Services.AddAlwaysAllowAuthorization();
            context.Services.Replace(ServiceDescriptor.Singleton<ICurrentPrincipalAccessor, FakeCurrentPrincipalAccessor>());
#endif

            //todo Cors?
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseErrorPage();
                app.UseHsts();
            }

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }
            app.UseAbpRequestLocalization(options =>
            {
                var defaultCulture = new CultureInfo("zh-hans");
                defaultCulture.DateTimeFormat.SetAllDateTimePatterns(new[] { "H:mm:ss" }, 'T');
                defaultCulture.DateTimeFormat.SetAllDateTimePatterns(new[] { "H:mm" }, 't');

                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
            });
            app.UseAuthorization();
            if (!env.IsProduction())
            {
                app.UseSwagger().UseAbpSwaggerUI(options =>
                {
                    Array.ForEach(HostApiGroup, p =>
                     {
                         options.SwaggerEndpoint($"/swagger/{p.Name}/swagger.json", p.DisplayName);
                     });

                    options.DefaultModelsExpandDepth(-1);
                    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

                    var configuration = context.GetConfiguration();
                    options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                    options.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                });

                app.UseMiniProfiler();
                app.UseHangfireDashboard();
                app.UseCapDashboard();
            }
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            SeedData(context);
        }

        private void SeedData(ApplicationInitializationContext context)
        {
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
    }
}
