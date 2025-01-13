using abp_outer_module;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace abp_console
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(OuterModule)
    )]
    internal class ConsoleModule : AbpModule
    {
        
    }
}
