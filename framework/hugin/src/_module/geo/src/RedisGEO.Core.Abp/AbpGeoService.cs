using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Microsoft.Extensions.Options;
using RedisGEO.Core;
using RedisGEO.Core.Models;

namespace RedisGEO.Abp
{
    public class AbpGeoService : ISingletonDependency, IAbpGeoService
    {
        private readonly IGeoService _geoService;

        public AbpGeoService(string connectionString)
        {
            _geoService = new GeoService(connectionString);
        }

        public AbpGeoService(GeoConfig geoConfig) : this(geoConfig.ConnectionString) { }

        public AbpGeoService(IOptions<GeoConfig> options) : this(options.Value.ConnectionString) { }
    }
}
