using Microsoft.Extensions.Caching.Distributed;

namespace Yi.AspNetCore.Caching;

public interface IDistributedCache<TCacheItem> : IDistributedCache<TCacheItem, string>
    where TCacheItem : class
{
    IDistributedCache<TCacheItem, string> InternalCache { get; }
}

public interface IDistributedCache<TCacheItem, TCacheKey>
    where TCacheItem : class
{
    TCacheItem? Get(
        TCacheKey key
    );
    
    Task<TCacheItem?> GetAsync(
        TCacheKey key,
        CancellationToken token = default
    );
    
    TCacheItem? GetOrAdd(
        TCacheKey key,
        Func<TCacheItem> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null
    );
    
    Task<TCacheItem?> GetOrAddAsync(
        TCacheKey key,
        Func<Task<TCacheItem>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null,
        CancellationToken token = default
    );
    
    void Set(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions? options = null
    );
    
    Task SetAsync(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions? options = null,
        CancellationToken token = default
    );
    
    void Refresh(
        TCacheKey key,
        bool? hideErrors = null
    );
    
    Task RefreshAsync(
        TCacheKey key,
        CancellationToken token = default
    );
    
    void Remove(
        TCacheKey key
    );
    
    Task RemoveAsync(
        TCacheKey key,
        CancellationToken token = default
    );
}
