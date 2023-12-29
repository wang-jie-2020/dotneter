using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RedisGEO.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseGeoRedis(this IServiceCollection services, Action<GeoConfig> configure)
        {
            services.TryAddSingleton<IGeoService, GeoService>();

            if (configure != null)
            {
                services.Configure(configure);
            }

            return services;
        }

        public static IServiceCollection UseGeoRedis(this IServiceCollection services, string connectionString)
        {
            return services.UseGeoRedis(options =>
            {
                options.ConnectionString = connectionString;
            });
        }
    }
}
