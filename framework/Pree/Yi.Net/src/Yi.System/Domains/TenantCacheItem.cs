﻿using Yi.AspNetCore.MultiTenancy;

namespace Yi.System.Domains;

[Serializable]
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
        if (id == null && name.IsNullOrWhiteSpace()) throw new Exception("Both id and name can't be invalid.");

        return string.Format(CacheKeyFormat,
            id?.ToString() ?? "null",
            name.IsNullOrWhiteSpace() ? "null" : name);
    }
}