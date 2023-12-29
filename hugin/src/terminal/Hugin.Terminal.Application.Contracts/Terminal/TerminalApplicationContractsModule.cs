using Hugin.Platform;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Hugin.Terminal
{
    [DependsOn(
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(TerminalDomainSharedModule),
        typeof(PlatformDomainSharedModule)
        )]
    public class TerminalApplicationContractsModule : AbpModule
    {

    }
}
