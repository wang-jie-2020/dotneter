using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore;
using Volo.Abp.Caching;
using Volo.Abp.ObjectMapping;
using Yi.Framework.Caching.FreeRedis;
using Yi.Framework.Mapster;

namespace Yi.Framework;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpObjectMappingModule), 
    typeof(AbpCachingModule))]
public class YiAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        //Mapster
        context.Services.AddTransient<IAutoObjectMappingProvider, MapsterAutoObjectMappingProvider>();
        
        //Redis
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