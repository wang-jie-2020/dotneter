using CSRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedisGEO.Core.Models;
using Microsoft.Extensions.Options;

namespace RedisGEO.Core
{
    public class GeoService : IGeoService
    {
        private readonly CSRedisClient _client;

        public GeoService(string connectionString)
        {
            this._client = new CSRedisClient(connectionString);
        }

        public GeoService(GeoConfig geoConfig) : this(geoConfig.ConnectionString) { }

        public GeoService(IOptions<GeoConfig> options) : this(options.Value.ConnectionString) { }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _client.ExistsAsync(key);
        }

        public async Task<long> CountAsync(string key)
        {
            return await _client.ZCardAsync(key);
        }

        public async Task<bool> AddAsync(string key, string name, decimal longitude, decimal latitude)
        {
            return await _client.GeoAddAsync(key, longitude, latitude, name);
        }

        public async Task<long> DeleteAsync(string key, string name)
        {
            return await _client.ZRemAsync(key, name);
        }

        public async Task<long> BatchDeleteAsync(string key)
        {
            return await _client.DelAsync(key);
        }

        public async Task<IEnumerable<RadiusWithDistResult>> RadiusWithDistAsync(RadiusWithDistInput pointInput)
        {
            var values = await _client.GeoRadiusWithDistAsync(pointInput.Key
                , pointInput.Longitude
                , pointInput.Latitude
                , pointInput.Radius
                , pointInput.Unit
                , pointInput.Count
                , pointInput.GeoOrderBy);

            var rwds = from v in values.ToList()
                       select new RadiusWithDistResult()
                       {
                           Name = v.member,
                           Dist = v.dist
                       };
            return rwds.ToList();
        }
    }
}
