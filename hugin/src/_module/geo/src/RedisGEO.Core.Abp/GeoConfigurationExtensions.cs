using System;
using Abp.Configuration.Startup;
using Abp.Dependency;
using RedisGEO.Core;

namespace RedisGEO.Abp
{
    public static class GeoConfigurationExtensions
    {
        public static void UseAbpGeoRedis(this IAbpStartupConfiguration configuration, Action<GeoConfig> optionsAction)
        {
            optionsAction(configuration.IocManager.Resolve<GeoConfig>());
        }

        public static void UseAbpGeoRedis(this IAbpStartupConfiguration configuration, string connectionString)
        {
            configuration.UseAbpGeoRedis(o =>
            {
                configuration.IocManager.Resolve<GeoConfig>().ConnectionString = connectionString;
            });
        }
    }
}
