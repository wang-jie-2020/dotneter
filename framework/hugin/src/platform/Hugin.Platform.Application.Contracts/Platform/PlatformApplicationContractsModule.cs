using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Hugin.Platform
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(PlatformDomainSharedModule)
        )]
    public class PlatformApplicationContractsModule : AbpModule
    {

    }
}
