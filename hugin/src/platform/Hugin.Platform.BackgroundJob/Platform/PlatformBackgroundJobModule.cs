﻿using Volo.Abp.Modularity;

namespace Hugin.Platform
{
    [DependsOn(
        typeof(PlatformDomainModule)
    )]
    public class PlatformBackgroundJobModule : AbpModule
    {
    }
}
