using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Yi.AspNetCore.Caching;

/// <summary>
/// Represents a distributed cache of <typeparamref name="TCacheItem" /> type.
/// </summary>
/// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
public class DistributedCache<TCacheItem> :
    IDistributedCache<TCacheItem>
    where TCacheItem : class
{
    public IDistributedCache<TCacheItem, string> InternalCache { get; }

    public DistributedCache(IDistributedCache<TCacheItem, string> internalCache)
    {
        InternalCache = internalCache;
    }

    public TCacheItem? Get(string key, bool? hideErrors = null, bool considerUow = false)
    {
        return InternalCache.Get(key, hideErrors, considerUow);
    }

    public Task<TCacheItem?> GetAsync(string key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        return InternalCache.GetAsync(key, hideErrors, considerUow, token);
    }

    public TCacheItem? GetOrAdd(string key, Func<TCacheItem> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null, bool? hideErrors = null, bool considerUow = false)
    {
        return InternalCache.GetOrAdd(key, factory, optionsFactory, hideErrors, considerUow);
    }

    public Task<TCacheItem?> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        return InternalCache.GetOrAddAsync(key, factory, optionsFactory, hideErrors, considerUow, token);
    }

    public void Set(string key, TCacheItem value, DistributedCacheEntryOptions? options = null, bool? hideErrors = null, bool considerUow = false)
    {
        InternalCache.Set(key, value, options, hideErrors, considerUow);
    }

    public Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions? options = null, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        return InternalCache.SetAsync(key, value, options, hideErrors, considerUow, token);
    }

    public void Refresh(string key, bool? hideErrors = null)
    {
        InternalCache.Refresh(key, hideErrors);
    }

    public Task RefreshAsync(string key, bool? hideErrors = null, CancellationToken token = default)
    {
        return InternalCache.RefreshAsync(key, hideErrors, token);
    }

    public void Remove(string key, bool? hideErrors = null, bool considerUow = false)
    {
        InternalCache.Remove(key, hideErrors, considerUow);
    }

    public Task RemoveAsync(string key, bool? hideErrors = null, bool considerUow = false, CancellationToken token = default)
    {
        return InternalCache.RemoveAsync(key, hideErrors, considerUow, token);
    }
}

/// <summary>
/// Represents a distributed cache of <typeparamref name="TCacheItem" /> type.
/// Uses a generic cache key type of <typeparamref name="TCacheKey" /> type.
/// </summary>
/// <typeparam name="TCacheItem">The type of cache item being cached.</typeparam>
/// <typeparam name="TCacheKey">The type of cache key being used.</typeparam>
public class DistributedCache<TCacheItem, TCacheKey> : IDistributedCache<TCacheItem, TCacheKey>
    where TCacheItem : class
    where TCacheKey : notnull
{
    public const string UowCacheName = "AbpDistributedCache";

    public ILogger<DistributedCache<TCacheItem, TCacheKey>> Logger { get; set; }

    protected IDistributedCache Cache { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    protected IDistributedCacheSerializer Serializer { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected SemaphoreSlim SyncSemaphore { get; }

    protected DistributedCacheEntryOptions DefaultCacheOptions = default!;

    private readonly AbpDistributedCacheOptions _distributedCacheOption;

    public DistributedCache(
        IOptions<AbpDistributedCacheOptions> distributedCacheOption,
        IDistributedCache cache,
        ICancellationTokenProvider cancellationTokenProvider,
        IDistributedCacheSerializer serializer,
        IServiceScopeFactory serviceScopeFactory,
        IUnitOfWorkManager unitOfWorkManager)
    {
        _distributedCacheOption = distributedCacheOption.Value;
        Cache = cache;
        CancellationTokenProvider = cancellationTokenProvider;
        Logger = NullLogger<DistributedCache<TCacheItem, TCacheKey>>.Instance;
        Serializer = serializer;
        ServiceScopeFactory = serviceScopeFactory;
        UnitOfWorkManager = unitOfWorkManager;

        SyncSemaphore = new SemaphoreSlim(1, 1);

        SetDefaultOptions();
    }

    protected virtual string NormalizeKey(TCacheKey key)
    {
        return key.ToString()!;
    }

    protected virtual DistributedCacheEntryOptions GetDefaultCacheEntryOptions()
    {
        return _distributedCacheOption.GlobalCacheEntryOptions;
    }

    protected virtual void SetDefaultOptions()
    {
        //Configure default cache entry options
        DefaultCacheOptions = GetDefaultCacheEntryOptions();
    }

    /// <summary>
    /// Gets a cache item with the given key. If no cache item is found for the given key then returns null.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <returns>The cache item, or null.</returns>
    public virtual TCacheItem? Get(
        TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false)
    {
        byte[]? cachedBytes = Cache.Get(NormalizeKey(key));
        return ToCacheItem(cachedBytes);
    }

    /// <summary>
    /// Gets a cache item with the given key. If no cache item is found for the given key then returns null.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The cache item, or null.</returns>
    public virtual async Task<TCacheItem?> GetAsync(
        TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default)
    {
        byte[]? cachedBytes = await Cache.GetAsync(
                NormalizeKey(key),
                CancellationTokenProvider.FallbackToProvider(token));

        if (cachedBytes == null)
        {
            return null;
        }

        return Serializer.Deserialize<TCacheItem>(cachedBytes);
    }

    /// <summary>
    /// Gets or Adds a cache item with the given key. If no cache item is found for the given key then adds a cache item
    /// provided by <paramref name="factory" /> delegate and returns the provided cache item.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache item when no cache item is found for the given <paramref name="key" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <returns>The cache item.</returns>
    public virtual TCacheItem? GetOrAdd(
        TCacheKey key,
        Func<TCacheItem> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null,
        bool? hideErrors = null,
        bool considerUow = false)
    {
        var value = Get(key, hideErrors, considerUow);
        if (value != null)
        {
            return value;
        }

        using (SyncSemaphore.Lock())
        {
            value = Get(key, hideErrors, considerUow);
            if (value != null)
            {
                return value;
            }

            value = factory();

            Set(key, value, optionsFactory?.Invoke(), hideErrors, considerUow);
        }

        return value;
    }

    /// <summary>
    /// Gets or Adds a cache item with the given key. If no cache item is found for the given key then adds a cache item
    /// provided by <paramref name="factory" /> delegate and returns the provided cache item.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="factory">The factory delegate is used to provide the cache item when no cache item is found for the given <paramref name="key" />.</param>
    /// <param name="optionsFactory">The cache options for the factory delegate.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The cache item.</returns>
    public virtual async Task<TCacheItem?> GetOrAddAsync(
        TCacheKey key,
        Func<Task<TCacheItem>> factory,
        Func<DistributedCacheEntryOptions>? optionsFactory = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default)
    {
        token = CancellationTokenProvider.FallbackToProvider(token);
        var value = await GetAsync(key, hideErrors, considerUow, token);
        if (value != null)
        {
            return value;
        }

        using (await SyncSemaphore.LockAsync(token))
        {
            value = await GetAsync(key, hideErrors, considerUow, token);
            if (value != null)
            {
                return value;
            }

            value = await factory();

            await SetAsync(key, value, optionsFactory?.Invoke(), hideErrors, considerUow, token);
        }

        return value;
    }

    /// <summary>
    /// Sets the cache item value for the provided key.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="value">The cache item value to set in the cache.</param>
    /// <param name="options">The cache options for the value.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    public virtual void Set(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions? options = null,
        bool? hideErrors = null,
        bool considerUow = false)
    {
        Cache.Set(
            NormalizeKey(key),
            Serializer.Serialize(value),
            options ?? DefaultCacheOptions
        );
    }
    /// <summary>
    /// Sets the cache item value for the provided key.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="value">The cache item value to set in the cache.</param>
    /// <param name="options">The cache options for the value.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    public virtual async Task SetAsync(
        TCacheKey key,
        TCacheItem value,
        DistributedCacheEntryOptions? options = null,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default)
    {
        await Cache.SetAsync(
            NormalizeKey(key),
            Serializer.Serialize(value),
            options ?? DefaultCacheOptions,
            CancellationTokenProvider.FallbackToProvider(token)
        );
    }

    /// <summary>
    /// Refreshes the cache value of the given key, and resets its sliding expiration timeout.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    public virtual void Refresh(
        TCacheKey key,
        bool? hideErrors = null)
    {
        Cache.Refresh(NormalizeKey(key));
    }

    /// <summary>
    /// Refreshes the cache value of the given key, and resets its sliding expiration timeout.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    public virtual async Task RefreshAsync(
        TCacheKey key,
        bool? hideErrors = null,
        CancellationToken token = default)
    {
        await Cache.RefreshAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
    }

    /// <summary>
    /// Removes the cache item for given key from cache.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    public virtual void Remove(
        TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false)
    {
        Cache.Remove(NormalizeKey(key));
    }

    /// <summary>
    /// Removes the cache item for given key from cache.
    /// </summary>
    /// <param name="key">The key of cached item to be retrieved from the cache.</param>
    /// <param name="hideErrors">Indicates to throw or hide the exceptions for the distributed cache.</param>
    /// <param name="considerUow">This will store the cache in the current unit of work until the end of the current unit of work does not really affect the cache.</param>
    /// <param name="token">The <see cref="T:System.Threading.CancellationToken" /> for the task.</param>
    /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> indicating that the operation is asynchronous.</returns>
    public virtual async Task RemoveAsync(
        TCacheKey key,
        bool? hideErrors = null,
        bool considerUow = false,
        CancellationToken token = default)
    {
        await Cache.RemoveAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
    }

    protected virtual void HandleException(Exception ex)
    {
        _ = HandleExceptionAsync(ex);
    }

    protected virtual async Task HandleExceptionAsync(Exception ex)
    {
        Logger.LogException(ex, LogLevel.Warning);

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            await scope.ServiceProvider
                .GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(new ExceptionNotificationContext(ex, LogLevel.Warning));
        }
    }

    protected virtual KeyValuePair<TCacheKey, TCacheItem?>[] ToCacheItems(byte[]?[] itemBytes, TCacheKey[] itemKeys)
    {
        if (itemBytes.Length != itemKeys.Length)
        {
            throw new AbpException("count of the item bytes should be same with the count of the given keys");
        }

        var result = new List<KeyValuePair<TCacheKey, TCacheItem?>>();

        for (var i = 0; i < itemKeys.Length; i++)
        {
            result.Add(
                new KeyValuePair<TCacheKey, TCacheItem?>(
                    itemKeys[i],
                    ToCacheItem(itemBytes[i])
                )
            );
        }

        return result.ToArray();
    }

    protected virtual TCacheItem? ToCacheItem(byte[]? bytes)
    {
        if (bytes == null)
        {
            return null;
        }

        return Serializer.Deserialize<TCacheItem>(bytes);
    }

    protected virtual KeyValuePair<string, byte[]>[] ToRawCacheItems(KeyValuePair<TCacheKey, TCacheItem>[] items)
    {
        return items
            .Select(i => new KeyValuePair<string, byte[]>(
                    NormalizeKey(i.Key),
                    Serializer.Serialize(i.Value)
                )
            ).ToArray();
    }

    private static KeyValuePair<TCacheKey, TCacheItem?>[] ToCacheItemsWithDefaultValues(TCacheKey[] keys)
    {
        return keys
            .Select(key => new KeyValuePair<TCacheKey, TCacheItem?>(key, default))
            .ToArray();
    }
}
