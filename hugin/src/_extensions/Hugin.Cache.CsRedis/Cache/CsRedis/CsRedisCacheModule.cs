using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Hugin.Cache.CsRedis
{
    [DependsOn(
        typeof(AbpCachingModule)
    )]
    public class CsRedisCacheModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.Configure<CsRedisCacheOptions>(cacheOptions =>
            {
                var redisConfiguration = configuration["Redis:Configuration"];
                if (!redisConfiguration.IsNullOrEmpty())
                {
                    cacheOptions.ConnectionString = redisConfiguration;
                }
            });
            context.Services.AddSingleton(typeof(ICsRedisCache), typeof(CsRedisCache));
            context.Services.AddSingleton(typeof(ICsRedisCache<>), typeof(CsRedisCache<>));
            context.Services.AddSingleton(typeof(ICsRedisCache<,>), typeof(CsRedisCache<,>));
        }
    }
}
