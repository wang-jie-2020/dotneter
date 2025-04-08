using Volo.Abp.Modularity;

namespace Hugin.Terminal
{
    [DependsOn(
        typeof(TerminalDomainModule)
    )]
    public class TerminalCapModule : AbpModule
    {
    }
}
