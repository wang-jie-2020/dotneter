using System.Text;
using FreeRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using My.Extensions.Localization.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp.Autofac;
using Volo.Abp.Uow;
using Yi.AspNetCore.Data;
using Yi.AspNetCore.Data.Filtering;
using Yi.AspNetCore.Data.Seeding;
using Yi.AspNetCore.MultiTenancy;
using Yi.AspNetCore.Mvc;
using Yi.AspNetCore.Mvc.Conventions;
using Yi.AspNetCore.Mvc.ExceptionHandling;
using Yi.AspNetCore.Threading;

namespace Yi.AspNetCore;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpUnitOfWorkModule)
)]
public class YiAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        RegisterDataSeedContributors(context.Services);
    }

    private static void RegisterDataSeedContributors(IServiceCollection services)
    {
        var contributors = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IDataSeedContributor).IsAssignableFrom(context.ImplementationType))
            {
                contributors.Add(context.ImplementationType);
            }
        });

        services.Configure<DataSeedOptions>(options =>
        {
            options.Contributors.AddIfNotContains(contributors);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        // AspNetCore & Mvc
        context.Services.AddHttpContextAccessor();
        context.Services.AddObjectAccessor<IApplicationBuilder>();
        context.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        context.Services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            })
            .AddDataAnnotationsLocalization()
            .AddControllersAsServices();

        Configure<MvcOptions>(options =>
        {
            options.Conventions.Add(new ControllerGroupNameConvention());

            options.Filters.AddService<UowActionFilter>();
            options.Filters.AddService<ExceptionFilter>();
        });

        Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var defaultLocalizer = actionContext.HttpContext.RequestServices.GetRequiredService<IStringLocalizer>();

                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                var detailBuilder = new StringBuilder();
                detailBuilder.Append(defaultLocalizer["ValidationNarrativeErrorMessageTitle"]);
                foreach (var error in errors)
                {
                    detailBuilder.AppendLine(error.Value.JoinAsString(","));
                }

                var response = AjaxResult.Error(defaultLocalizer["ValidationErrorMessage"], detailBuilder.ToString());

                return new BadRequestObjectResult(response);
            };
        });

        // Localization  
        context.Services.AddJsonLocalization(options => options.ResourcesPath = "Resources");
        context.Services.Replace(new ServiceDescriptor(typeof(IStringLocalizerFactory), typeof(JsonStringLocalizerFactory), ServiceLifetime.Singleton));    // WTF --> SEE Volo.Abp.Internal.InternalServiceCollectionExtensions.AddCoreServices

        // MemoryCache & Redis
        context.Services.AddMemoryCache();
        context.Services.AddDistributedMemoryCache();

        var redisEnabled = configuration["Redis:IsEnabled"];
        if (!redisEnabled.IsNullOrEmpty() && bool.Parse(redisEnabled))
        {
            var redisConfiguration = configuration["Redis:ConnectionString"];
            var redisClient = new RedisClient(redisConfiguration);

            context.Services.AddSingleton<IRedisClient>(redisClient);
            context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache>(new DistributedCache(redisClient)));
        }

        // Other
        Configure<DbConnectionOptions>(configuration);

        context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        context.Services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);
        context.Services.AddSingleton(typeof(IAmbientScopeProvider<>), typeof(AmbientDataContextAmbientScopeProvider<>));

        context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
        context.Services.AddTransient<IConfigureOptions<SwaggerUIOptions>, SwaggerConfigureOptions>();
    }
}