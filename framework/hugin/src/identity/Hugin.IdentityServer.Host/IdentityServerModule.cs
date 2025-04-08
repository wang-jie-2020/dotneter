using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Hugin.IdentityServer.IdentityMappingExtensions;
using Hugin.IdentityServer.Security;
using Hugin.Mvc;
using Hugin.Platform;
using Hugin.Terminal;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.Threading;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Hugin.IdentityServer
{
    [DependsOn(
        //引入服务
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AbpAutofacModule),
        typeof(AbpCachingStackExchangeRedisModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpSwashbuckleModule),
        typeof(HuginMvcModule),
        //引入模块
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpAccountWebIdentityServerModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpFeatureManagementWebModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(AbpTenantManagementWebModule),
        //引入项目
        typeof(PlatformApplicationContractsModule),
        typeof(TerminalApplicationContractsModule)
        )]
    public class IdentityServerModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        private static readonly ApiInfo[] HostApiGroup = new[] { ApiGroups.AbpGroup, ApiGroups.IdentityGroup };

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            IdentityEntityMappingExtensions.Configure();
            IdentityDtoMappingExtensions.Configure();

            PreConfigure<AbpIdentityServerBuilderOptions>(options =>
            {
                options.AddDeveloperSigningCredential = false;
            });

            PreConfigure<IIdentityServerBuilder>(builder =>
            {
                //Certificate
                var certificate = Assembly.GetEntryAssembly().GetManifestResourceStream(typeof(IdentityServerModule).Namespace + ".ids4.pfx");
                if (certificate != null)
                {
                    var buffer = new byte[certificate.Length];
                    certificate.Read(buffer, 0, buffer.Length);

                    builder.AddSigningCredential(new X509Certificate2(buffer, "lg"));
                }

                //UserClaims
                builder.Services.Replace(ServiceDescriptor.Transient<IObjectAccessor<IUserClaimsPrincipalFactory<IdentityUser>>, ObjectAccessor<HuginUserClaimsPrincipalFactory>>());
                builder.Services.Replace(ServiceDescriptor.Transient<IClaimsService, HuginClaimsService>());
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseMySQL(opt =>
                {
                    opt.UseQuerySplittingBehavior(Microsoft.EntityFrameworkCore.QuerySplittingBehavior.SingleQuery);
                });
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });

            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "Identity:";
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });

            var authenticationBuilder = context.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = configuration["AuthServer:Audience"];
                });

            Configure<AbpAuditingOptions>(options =>
            {
                options.ApplicationName = "AuthServer";
            });

            context.Services.AddAutoMapperObjectMapper<IdentityServerModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<IdentityServerModule>();
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

            context.Services.AddSwaggerGen(
                options =>
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
                });

            context.Services.AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; }).AddEntityFramework();

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
                app.UseErrorPage();
                app.UseHsts();
            }

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                /*  
                 *  SameSitePolicy = None --> http request error
                 *  see https://docs.microsoft.com/zh-cn/aspnet/core/security/samesite?view=aspnetcore-5.0
                 */
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
            });

            app.UseAuthentication();
            app.UseJwtTokenMiddleware();
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
            app.UseIdentityServer();
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
                });

                app.UseMiniProfiler();
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
