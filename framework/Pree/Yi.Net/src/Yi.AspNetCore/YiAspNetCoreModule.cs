using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Autofac;
using Volo.Abp.Uow;

namespace Yi.AspNetCore;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpUnitOfWorkModule)
)]
public class YiAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {

    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

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