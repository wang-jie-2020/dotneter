using FreeRedis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.Uow;
using Yi.AspNetCore.Data;
using Yi.AspNetCore.Data.Filtering;
using Yi.AspNetCore.Data.Seeding;

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

        context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        
        context.Services.AddJsonLocalization(options => options.ResourcesPath = "Resources");
        
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
    }
}