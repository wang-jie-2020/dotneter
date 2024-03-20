using Volo.Abp.Modularity;

namespace AESC.Starter
{
    [DependsOn(
        typeof(StarterApplicationModule),
        typeof(StarterDomainTestModule)
        )]
    public class StarterApplicationTestModule : AbpModule
    {

    }
}