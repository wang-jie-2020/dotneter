using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Yi.System.Domains.Sys.Entities;
using Yi.System.Domains.Sys.Repositories;

namespace Yi.System.Domains.Sys;

public class SqlSugarAndConfigurationTenantStore : DefaultTenantStore, ITenantStore
{
    public SqlSugarAndConfigurationTenantStore(ISqlSugarTenantRepository repository,
        IDistributedCache<TenantCacheItem> cache,
        ICurrentTenant currentTenant,
        IOptionsMonitor<AbpDefaultTenantStoreOptions> options) : base(options)
    {
        TenantRepository = repository;
        Cache = cache;
        CurrentTenant = currentTenant;
    }

    private ISqlSugarTenantRepository TenantRepository { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedCache<TenantCacheItem> Cache { get; }

    public new TenantConfiguration? Find(string name)
    {
        throw new NotImplementedException("请使用异步方法");
    }

    public new TenantConfiguration? Find(Guid id)
    {
        throw new NotImplementedException("请使用异步方法");
    }

    public new async Task<TenantConfiguration?> FindAsync(string name)
    {
        var tenantFromOptions = await base.FindAsync(name);
        if (tenantFromOptions is null)
        {
            return (await GetCacheItemAsync(null, name)).Value;
        }
        return tenantFromOptions;
    }

    public new async Task<TenantConfiguration?> FindAsync(Guid id)
    {
        var tenantFromOptions = await base.FindAsync(id);
        if (tenantFromOptions is null)
        {
            return (await GetCacheItemAsync(id, null)).Value;
        }
        return tenantFromOptions;
    }


    protected virtual async Task<TenantCacheItem> GetCacheItemAsync(Guid? id, string? name)
    {
        var cacheKey = CalculateCacheKey(id, name);

        var cacheItem = await Cache.GetAsync(cacheKey, considerUow: true);
        if (cacheItem != null) return cacheItem;

        if (id.HasValue)
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantRepository.FindAsync(id.Value);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        if (!name.IsNullOrWhiteSpace())
        {
            using (CurrentTenant.Change(null)) //TODO: No need this if we can implement to define host side (or tenant-independent) entities!
            {
                var tenant = await TenantRepository.FindByNameAsync(name);
                return await SetCacheAsync(cacheKey, tenant);
            }
        }

        throw new AbpException("Both id and name can't be invalid.");
    }

    protected virtual async Task<TenantCacheItem> SetCacheAsync(string cacheKey, TenantEntity? tenant)
    {
        var tenantConfiguration = tenant != null ? MapToConfiguration(tenant) : null;
        var cacheItem = new TenantCacheItem(tenantConfiguration);
        await Cache.SetAsync(cacheKey, cacheItem, considerUow: true);
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
}