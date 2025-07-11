using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Data;
using Yi.AspNetCore.Extensions.Caching;
using Yi.AspNetCore.MultiTenancy;
using Yi.Framework.Core.Entities;
using Yi.Framework.SqlSugarCore.Repositories;

namespace Yi.Framework.Core;

public class TenantStore : ITenantStore, ITransientDependency
{
    protected ISqlSugarRepository<TenantEntity> TenantRepository { get; }
    protected IDistributedCache Cache { get; }
    protected ICurrentTenant CurrentTenant { get; }
    
    public TenantStore(
        ISqlSugarRepository<TenantEntity> repository,
        IDistributedCache cache,
        ICurrentTenant currentTenant)
    {
        TenantRepository = repository;
        Cache = cache;
        CurrentTenant = currentTenant;
    }
    
    public async Task<TenantConfiguration?> FindAsync(string name)
    {
        return (await GetCacheItemAsync(null, name)).Value;
    }

    public async Task<TenantConfiguration?> FindAsync(Guid id)
    {
        return (await GetCacheItemAsync(id, null)).Value;
    }

    protected virtual async Task<TenantCacheItem> GetCacheItemAsync(Guid? id, string? name)
    {
        var cacheKey = CalculateCacheKey(id, name);

        var cacheItem = await Cache.GetAsync<TenantCacheItem>(cacheKey);
        if (cacheItem != null) return cacheItem;

        if (id.HasValue)
        {
            using (CurrentTenant.Change(null))
            {
                var tenant = await TenantRepository.GetByIdAsync(id.Value);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        if (!name.IsNullOrWhiteSpace())
        {
            using (CurrentTenant.Change(null))
            {
                var tenant = await TenantRepository.AsQueryable().FirstAsync(x => x.Name == name);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        throw new Exception("Both id and name can't be invalid.");
    }

    protected virtual async Task<TenantCacheItem> SetCacheAsync(string cacheKey, TenantEntity? tenant)
    {
        var tenantConfiguration = tenant != null ? MapToConfiguration(tenant) : null;
        var cacheItem = new TenantCacheItem(tenantConfiguration);
        await Cache.SetAsync(cacheKey, cacheItem, new DistributedCacheEntryOptions());
        return cacheItem;
    }

    private TenantConfiguration MapToConfiguration(TenantEntity tenantEntity)
    {
        var tenantConfiguration = new TenantConfiguration
        {
            Id = tenantEntity.Id,
            Name = tenantEntity.Name,
            ConnectionStrings = MapToString(tenantEntity.TenantConnectionString),
            IsActive = true
        };
        return tenantConfiguration;
    }

    private ConnectionStrings? MapToString(string tenantConnectionString)
    {
        var connectionStrings = new ConnectionStrings
        {
            [ConnectionStrings.DefaultConnectionStringName] = tenantConnectionString
        };
        return connectionStrings;
    }

    protected virtual string CalculateCacheKey(Guid? id, string name)
    {
        return TenantCacheItem.CalculateCacheKey(id, name);
    }
    
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
}