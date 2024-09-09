using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Caching;

namespace Yi.Framework.Caching.FreeRedis;

[DependsOn(typeof(AbpCachingModule))]
public class YiFrameworkCachingFreeRedisModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        var redisEnabled = configuration["Redis:IsEnabled"];
        if (redisEnabled.IsNullOrEmpty() || bool.Parse(redisEnabled))
        {
            var redisConfiguration = configuration["Redis:Configuration"];
            var redisClient = new RedisClient(redisConfiguration);

            context.Services.AddSingleton<IRedisClient>(redisClient);
            context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache>(new DistributedCache(redisClient)));
            context.Services.Replace(ServiceDescriptor.Transient<IDistributedCacheKeyNormalizer, YiDistributedCacheKeyNormalizer>());
        }
    }
}