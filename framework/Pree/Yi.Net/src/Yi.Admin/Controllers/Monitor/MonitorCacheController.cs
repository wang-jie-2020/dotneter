using FreeRedis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Caching;
using Yi.Sys.Services.Monitor.Dtos;

namespace Yi.Admin.Controllers.Monitor;

[ApiController]
[Route("api/monitor/cache")]
public class MonitorCacheController : AbpController
{
    /// <summary>
    ///     缓存前缀
    /// </summary>
    private string CacheKeyPrefix => LazyServiceProvider.LazyGetRequiredService<IOptions<AbpDistributedCacheOptions>>().Value.KeyPrefix;

    private bool EnableRedisCache
    {
        get
        {
            var redisEnabled = LazyServiceProvider.LazyGetRequiredService<IConfiguration>()["Redis:IsEnabled"];
            return redisEnabled.IsNullOrEmpty() || bool.Parse(redisEnabled);
        }
    }

    /// <summary>
    ///     使用懒加载防止报错
    /// </summary>
    private IRedisClient RedisClient => LazyServiceProvider.LazyGetRequiredService<IRedisClient>();

    /// <summary>
    ///     获取所有key并分组
    /// </summary>
    /// <returns></returns>
    [HttpGet("name")]
    public List<MonitorCacheNameGetListOutputDto> GetName()
    {
        VerifyRedisCacheEnable();
        var keys = RedisClient.Keys(CacheKeyPrefix + "*");
        var result = GroupedKeys(keys.ToList());
        var output = result.Select(x => new MonitorCacheNameGetListOutputDto { CacheName = x }).ToList();
        return output;
    }

    private List<string> GroupedKeys(List<string> keys)
    {
        var resultSet = new HashSet<string>();
        foreach (var str in keys)
        {
            var parts = str.Split(':');

            // 如果字符串中包含冒号，则将第一部分和第二部分进行分组
            if (parts.Length >= 2)
            {
                var group = $"{parts[0]}:{parts[1]}";
                resultSet.Add(group);
            }
            // 如果字符串中不包含冒号，则直接进行分组
            else
            {
                resultSet.Add(str);
            }
        }

        return resultSet.ToList();
    }


    private void VerifyRedisCacheEnable()
    {
        if (!EnableRedisCache) throw new UserFriendlyException("后端程序未使用Redis缓存，无法对Redis进行监控，可切换使用Redis");
    }

    [HttpGet("key/{cacheName}")]
    public List<string> GetKey(string cacheName)
    {
        VerifyRedisCacheEnable();
        var output = RedisClient.Keys($"{cacheName}:*").Select(x => x.RemovePreFix(cacheName + ":"));
        return output.ToList();
    }

    //全部不为空
    [HttpGet("value/{cacheName}/{cacheKey}")]
    public MonitorCacheGetListOutputDto GetValue(string cacheName, string cacheKey)
    {
        var value = RedisClient.HGet($"{cacheName}:{cacheKey}", "data");
        return new MonitorCacheGetListOutputDto { CacheKey = cacheKey, CacheName = cacheName, CacheValue = value };
    }


    [HttpDelete("key/{cacheName}")]
    public bool DeleteKey(string cacheName)
    {
        VerifyRedisCacheEnable();
        RedisClient.Del($"{cacheName}:*");
        return true;
    }

    [HttpDelete("value/{cacheName}/{cacheKey}")]
    public bool DeleteValue(string cacheName, string cacheKey)
    {
        RedisClient.Del($"{cacheName}:{cacheKey}");
        return true;
    }

    [HttpDelete("clear")]
    public bool DeleteClear()
    {
        RedisClient.FlushDb();
        return true;
    }
}