using Volo.Abp.Modularity;

namespace Hugin.Terminal
{
    [DependsOn(
        typeof(TerminalDomainModule)
    )]
    public class TerminalBackgroundJobModule : AbpModule
    {
    }
}
