﻿using Volo.Abp.MultiTenancy;

namespace Yi.Sys.Domains.Infra;

[Serializable]
[IgnoreMultiTenancy]
public class TenantCacheItem
{
    private const string CacheKeyFormat = "i:{0},n:{1}";

    public TenantCacheItem()
    {
    }

    public TenantCacheItem(TenantConfiguration value)
    {
        Value = value;
    }

    public TenantConfiguration Value { get; set; }

    public static string CalculateCacheKey(Guid? id, string name)
    {
        if (id == null && name.IsNullOrWhiteSpace()) throw new AbpException("Both id and name can't be invalid.");

        return string.Format(CacheKeyFormat,
            id?.ToString() ?? "null",
            name.IsNullOrWhiteSpace() ? "null" : name);
    }
}