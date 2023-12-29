using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace Hugin.Cache.CsRedis
{
    /*
     *  Note:不过度封装，封装内容只是考虑技术水平和常用
     */
    public class CsRedisCache : ICsRedisCache
    {
        protected readonly CsRedisCacheOptions CsRedisCacheOptions;
        protected readonly AbpDistributedCacheOptions DistributedCacheOption;
        protected readonly IDistributedCacheKeyNormalizer KeyNormalizer;
        protected readonly IDistributedCacheSerializer Serializer;
        protected CSRedisCache CsRedisDistributedCache; //CsRedis实现的IDistributedCache，这里只是考虑到Get-And-Refresh的借用

        /// <summary>
        /// CsRedis客户端，不含任何封装
        /// </summary>
        public CSRedisClient CsRedisClient { get; }

        public CsRedisCache(IOptions<CsRedisCacheOptions> csRedisCacheOptions,
                IOptions<AbpDistributedCacheOptions> distributedCacheOptions,
                IDistributedCacheKeyNormalizer keyNormalizer,
                IDistributedCacheSerializer serializer)
        {
            CsRedisCacheOptions = csRedisCacheOptions.Value;
            DistributedCacheOption = distributedCacheOptions.Value;
            KeyNormalizer = keyNormalizer;
            Serializer = serializer;

            CsRedisClient = CsRedisClientFactory.CreateClient(CsRedisCacheOptions.ConnectionString);
            CsRedisDistributedCache = new CSRedisCache(CsRedisClient);
        }

        /// <summary>
        /// redis key
        /// </summary>
        /// <param name="key">键中唯一内容</param>
        /// <param name="cacheName">键名</param>
        /// <param name="ignoreMultiTenancy">忽略租户</param>
        /// <returns>t:租户-c:键名-k:(键前缀+键中唯一内容)</returns>
        public string NormalizeKey([NotNull] string key, [NotNull] string cacheName, bool ignoreMultiTenancy)
        {
            return KeyNormalizer.NormalizeKey(
                new DistributedCacheKeyNormalizeArgs(
                    key,
                    cacheName,
                    ignoreMultiTenancy
                )
            );
        }
    }

    public class CsRedisCache<TCacheItem> : CsRedisCache<TCacheItem, string>, ICsRedisCache<TCacheItem> where TCacheItem : class
    {
        public CsRedisCache(IOptions<CsRedisCacheOptions> csRedisCacheOptions,
                IOptions<AbpDistributedCacheOptions> distributedCacheOptions,
                IDistributedCacheKeyNormalizer keyNormalizer,
                IDistributedCacheSerializer serializer) : base(csRedisCacheOptions, distributedCacheOptions, keyNormalizer, serializer)
        {

        }
    }

    /*
     * DistributedCacheEntryOptions已经由Caching.CsRedis包实现了，不想造轮子
     * 基于Caching.CsRedis和CsRedisCore来做一些顺序上的改动就可以了
     * Note：Caching.CsRedis中的序列化方法System.Runtime.Serialization.Formatters.Binary.BinaryFormatter在net5已经不再支持
     */
    public class CsRedisCache<TCacheItem, TCacheKey> : CsRedisCache, ICsRedisCache<TCacheItem, TCacheKey> where TCacheItem : class
    {
        protected string CacheName;
        protected bool IgnoreMultiTenancy;
        protected DistributedCacheEntryOptions DefaultCacheOptions;

        public CsRedisCache(IOptions<CsRedisCacheOptions> csRedisCacheOptions,
                IOptions<AbpDistributedCacheOptions> distributedCacheOptions,
                IDistributedCacheKeyNormalizer keyNormalizer,
                IDistributedCacheSerializer serializer) : base(csRedisCacheOptions, distributedCacheOptions, keyNormalizer, serializer)
        {
            SetDefaultOptions();
        }

        protected void SetDefaultOptions()
        {
            CacheName = CacheNameAttribute.GetCacheName(typeof(TCacheItem));
            IgnoreMultiTenancy = typeof(TCacheItem).IsDefined(typeof(IgnoreMultiTenancyAttribute), true);
            DefaultCacheOptions = GetDefaultCacheEntryOptions();
        }

        protected DistributedCacheEntryOptions GetDefaultCacheEntryOptions()
        {
            foreach (var configure in DistributedCacheOption.CacheConfigurators)
            {
                var options = configure.Invoke(CacheName);
                if (options != null)
                {
                    return options;
                }
            }

            return DistributedCacheOption.GlobalCacheEntryOptions;
        }

        /// <summary>
        /// redis key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string NormalizeKey(TCacheKey key)
        {
            return NormalizeKey(
                key.ToString(),
                CacheName,
                IgnoreMultiTenancy);
        }

        /// <summary>
        /// key-object
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<TCacheItem> GetAsync(TCacheKey key)
        {
            var value = await CsRedisDistributedCache.GetAsync(NormalizeKey(key));
            return value == null ? null : Serializer.Deserialize<TCacheItem>(value);
        }

        /// <summary>
        /// key-object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        /// <param name="entryOptions"></param>
        /// <returns></returns>
        public async Task SetAsync(TCacheKey key, TCacheItem item, DistributedCacheEntryOptions entryOptions = null)
        {
            entryOptions ??= DefaultCacheOptions;

            var value = Serializer.Serialize(item);
            if (value == null)
            {
                await RemoveAsync(key);
                return;
            }

            await CsRedisDistributedCache.SetAsync(NormalizeKey(key), value, entryOptions);
        }

        /// <summary>
        /// key-object
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveAsync(TCacheKey key)
        {
            await CsRedisDistributedCache.RemoveAsync(NormalizeKey(key));
        }

        /// <summary>
        /// key-object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="optionsFactory"></param>
        /// <returns></returns>
        public async Task<TCacheItem> GetOrAddAsync(TCacheKey key, Func<Task<TCacheItem>> factory, Func<DistributedCacheEntryOptions> optionsFactory = null)
        {
            var value = await GetAsync(key);
            if (value != null)
            {
                return value;
            }

            value = await factory();
            await SetAsync(key, value, optionsFactory?.Invoke());
            return value;
        }

        /// <summary>
        /// key-field-object
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task<TCacheItem[]> HGetAsync(params string[] fields)
        {
            var key = NormalizeKey("", CacheName, IgnoreMultiTenancy);
            await CsRedisDistributedCache.GetAsync(key);

            return await CsRedisClient.HMGetAsync<TCacheItem>(key, fields);
        }

        /// <summary>
        /// key-field-object
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="items"></param>
        /// <param name="entryOptions"></param>
        /// <returns></returns>
        public async Task HSetAsync(string[] fields, TCacheItem[] items, DistributedCacheEntryOptions entryOptions = null)
        {
            if (fields.Length != items.Length)
            {
                throw new ArgumentException("键值对长度不一致");
            }

            entryOptions ??= DefaultCacheOptions;
            var key = NormalizeKey("", CacheName, IgnoreMultiTenancy);

            if ((await CsRedisDistributedCache.GetAsync(key)) == null)
            {
                await CsRedisDistributedCache.SetAsync(key, Encoding.UTF8.GetBytes("0"), entryOptions);
            }

            var keyValues = new object[fields.Length * 2];
            for (var index = 0; index < fields.Length; index++)
            {
                keyValues[index * 2] = fields[index];
                keyValues[index * 2 + 1] = items[index];
            }

            await CsRedisClient.HMSetAsync(key, keyValues);
        }

        /// <summary>
        /// key-field-object
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async Task HRemoveAsync(string[] fields)
        {
            var key = NormalizeKey("", CacheName, IgnoreMultiTenancy);
            await CsRedisClient.HDelAsync(key, fields);
        }

        /// <summary>
        /// key-object
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="factory"></param>
        /// <param name="optionsFactory"></param>
        /// <returns></returns>
        public async Task<TCacheItem[]> HGetOrAddAsync(string[] fields, Func<Task<TCacheItem[]>> factory, Func<DistributedCacheEntryOptions> optionsFactory = null)
        {
            var values = await HGetAsync(fields);
            if (values != null)
            {
                for (var index = 0; index < values.Length; index++)
                {
                    if (values[index] != null)
                    {
                        return values;
                    }
                }
            }

            values = await factory();
            await HSetAsync(fields, values, optionsFactory?.Invoke());    //DistributedCacheOption Needed EveryTime?
            return values;
        }
    }
}