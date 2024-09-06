namespace Yi.Abp.Infra.Rbac.Dtos.MonitorCache
{
    public class MonitorCacheGetListOutputDto
    {
        public string CacheName { get; set; }
        public string CacheKey { get; set; }
        public string CacheValue { get; set; }
        public string? Remark { get; set; }
    }
}
