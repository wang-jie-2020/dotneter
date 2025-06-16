using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Yi.AspNetCore.Caching;

public static class DistributedCacheExtensions
{
    public static T Get<T>(this IDistributedCache cache, string key) where T : class
    {
        byte[] bytes = cache.Get(key);
        if (bytes == null)
        {
            return null;
        }

        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
    }

    public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default(CancellationToken)) where T : class
    {
        byte[] bytes = await cache.GetAsync(key);
        if (bytes == null)
        {
            return null;
        }

        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
    }

    public static void Set<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
        cache.Set(key, bytes, options);
    }

    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
    {
        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
        await cache.SetAsync(key, bytes, options, token);
    }

    public static T GetOrAdd<T>(this IDistributedCache cache, string key, Func<T> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null) where T : class
    {
        var value = cache.Get<T>(key);
        if (value != null)
        {
            return value;
        }

        value = factory();

        cache.Set(key, value, optionsFactory?.Invoke());
        return value;
    }

    public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> factory, Func<DistributedCacheEntryOptions>? optionsFactory = null, CancellationToken token = default) where T : class
    {
        var value = await cache.GetAsync<T>(key, token);
        if (value != null)
        {
            return value;
        }

        value = await factory();

        await cache.SetAsync(key, value, optionsFactory?.Invoke(), token);
        return value;
    }
}