﻿using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Data;
using Yi.AspNetCore.Extensions.Caching;
using Yi.AspNetCore.MultiTenancy;
using Yi.System.Domains.Entities;
using Yi.System.Domains.Repositories;

namespace Yi.System.Domains;

public class SqlSugarAndConfigurationTenantStore : ITenantStore, ITransientDependency
{
    public SqlSugarAndConfigurationTenantStore(ISqlSugarTenantRepository repository,
        IDistributedCache cache,
        ICurrentTenant currentTenant)
    {
        TenantRepository = repository;
        Cache = cache;
        CurrentTenant = currentTenant;
    }

    private ISqlSugarTenantRepository TenantRepository { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IDistributedCache Cache { get; }

    public TenantConfiguration? Find(string name)
    {
        throw new NotImplementedException("请使用异步方法");
    }

    public TenantConfiguration? Find(Guid id)
    {
        throw new NotImplementedException("请使用异步方法");
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
}