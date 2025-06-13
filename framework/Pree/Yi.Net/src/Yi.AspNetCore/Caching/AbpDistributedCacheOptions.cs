using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Distributed;

namespace Yi.AspNetCore.Caching;

public class AbpDistributedCacheOptions
{
    public DistributedCacheEntryOptions GlobalCacheEntryOptions { get; set; }

    public AbpDistributedCacheOptions()
    {
        GlobalCacheEntryOptions = new DistributedCacheEntryOptions();
    }
}
