using abp_outer_module;
using Volo.Abp.Modularity;

namespace abp_app
{
    [DependsOn(
        typeof(OuterModule)
    )]
    internal class WebModule : AbpModule
    {
        
    }
}
