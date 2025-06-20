using FreeRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Yi.AspNetCore.Data;
using Yi.AspNetCore.Data.Filtering;
using Yi.AspNetCore.Data.Seeding;
using Yi.AspNetCore.MultiTenancy;
using Yi.AspNetCore.Mvc.ExceptionHandling;
using Yi.AspNetCore.Threading;
using Microsoft.Extensions.Localization;
using My.Extensions.Localization.Json;

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

        Configure<DbConnectionOptions>(configuration);

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

        // Localization --> SEE Volo.Abp.Internal.InternalServiceCollectionExtensions.AddCoreServices
        context.Services.AddJsonLocalization(options => options.ResourcesPath = "Resources");
        context.Services.Replace(new ServiceDescriptor(typeof(IStringLocalizerFactory), typeof(JsonStringLocalizerFactory), ServiceLifetime.Singleton));

        // AspNetCore & Mvc
        context.Services.AddHttpContextAccessor();
        context.Services.AddObjectAccessor<IApplicationBuilder>();
        context.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        
        context.Services.AddMvc()
            .AddDataAnnotationsLocalization().AddViewLocalization()
            .AddControllersAsServices().AddViewComponentsAsServices();
        
        context.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.AddService<ExceptionFilter>();
        });
        
        // Other
        context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        context.Services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);
        context.Services.AddSingleton(typeof(IAmbientScopeProvider<>), typeof(AmbientDataContextAmbientScopeProvider<>));
    }
}