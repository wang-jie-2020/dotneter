namespace RedisGEO.Core.Models
{
    public class AddInput
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }
    }
}
