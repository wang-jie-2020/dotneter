using Hugin.Platform.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Hugin.Platform
{
    [DependsOn(
        typeof(PlatformEntityFrameworkCoreTestModule)
        )]
    public class PlatformDomainTestModule : AbpModule
    {

    }
}
