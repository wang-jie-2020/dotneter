using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CSRedis;
using Microsoft.Extensions.Caching.Distributed;

namespace Hugin.Cache.CsRedis
{
    public interface ICsRedisCache
    {
        /// <summary>
        /// CsRedis客户端，不含任何封装
        /// </summary>
        CSRedisClient CsRedisClient { get; }

        /// <summary>
        /// redis key
        /// </summary>
        /// <param name="key">键中唯一内容</param>
        /// <param name="cacheName">键名</param>
        /// <param name="ignoreMultiTenancy">忽略租户</param>
        /// <returns>t:租户-c:键名-k:(键前缀+键中唯一内容)</returns>
        string NormalizeKey([NotNull] string key, [NotNull] string cacheName, bool ignoreMultiTenancy);
    }

    public interface ICsRedisCache<TCacheItem, in TCacheKey> : ICsRedisCache where TCacheItem : class
    {
        /// <summary>
        /// redis key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string NormalizeKey(TCacheKey key);

        /// <summary>
        /// key-object
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TCacheItem> GetAsync(TCacheKey key);

        /// <summary>
        /// key-object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="entryOptions"></param>
        /// <returns></returns>
        Task SetAsync(TCacheKey key, TCacheItem item, DistributedCacheEntryOptions entryOptions = null);

        /// <summary>
        /// key-object
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(TCacheKey key);

        /// <summary>
        /// key-object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="optionsFactory"></param>
        /// <returns></returns>
        Task<TCacheItem> GetOrAddAsync(TCacheKey key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions> optionsFactory = null);

        /// <summary>
        /// key-field-object
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task<TCacheItem[]> HGetAsync(string[] fields);

        /// <summary>
        /// key-field-object
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="items"></param>
        /// <param name="entryOptions"></param>
        /// <returns></returns>
        Task HSetAsync(string[] fields, TCacheItem[] items, DistributedCacheEntryOptions entryOptions = null);

        /// <summary>
        /// key-field-object
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        Task HRemoveAsync(string[] fields);

        /// <summary>
        /// key-filed-object
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="factory"></param>
        /// <param name="optionsFactory"></param>
        /// <returns></returns>
        Task<TCacheItem[]> HGetOrAddAsync(string[] fields, Func<Task<TCacheItem[]>> factory, Func<DistributedCacheEntryOptions> optionsFactory = null);
    }

    public interface ICsRedisCache<TCacheItem> : ICsRedisCache<TCacheItem, string> where TCacheItem : class
    {

    }
}
