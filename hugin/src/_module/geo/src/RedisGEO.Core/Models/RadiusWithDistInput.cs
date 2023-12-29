using CSRedis;

namespace RedisGEO.Core.Models
{
    public class RadiusWithDistInput
    {
        public string Key { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public decimal Radius { get; set; }

        public GeoUnit Unit { get; set; }

        public long? Count { get; set; }

        public GeoOrderBy? GeoOrderBy { get; set; }
    }
}
