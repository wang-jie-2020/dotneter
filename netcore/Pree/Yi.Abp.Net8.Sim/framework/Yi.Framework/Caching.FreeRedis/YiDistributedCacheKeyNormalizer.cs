using Microsoft.Extensions.Options;
using Volo.Abp.Caching;
using Volo.Abp.MultiTenancy;

namespace Yi.Framework.Caching.FreeRedis;

public class YiDistributedCacheKeyNormalizer : IDistributedCacheKeyNormalizer
{
    public YiDistributedCacheKeyNormalizer(
        ICurrentTenant currentTenant,
        IOptions<AbpDistributedCacheOptions> distributedCacheOptions)
    {
        CurrentTenant = currentTenant;
        DistributedCacheOptions = distributedCacheOptions.Value;
    }

    protected ICurrentTenant CurrentTenant { get; }

    protected AbpDistributedCacheOptions DistributedCacheOptions { get; }

    public virtual string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
    {
        var normalizedKey = $"{DistributedCacheOptions.KeyPrefix}{args.Key}";

        //if (!args.IgnoreMultiTenancy && CurrentTenant.Id.HasValue)
        //{
        //    normalizedKey = $"t:{CurrentTenant.Id.Value},{normalizedKey}";
        //}

        return normalizedKey;
    }
}