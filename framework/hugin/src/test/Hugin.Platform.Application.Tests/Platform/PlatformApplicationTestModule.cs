using Volo.Abp.Modularity;

namespace Hugin.Platform
{
    [DependsOn(
        typeof(PlatformApplicationModule),
        typeof(PlatformDomainTestModule)
    )]
    public class PlatformApplicationTestModule : AbpModule
    {

    }
}
