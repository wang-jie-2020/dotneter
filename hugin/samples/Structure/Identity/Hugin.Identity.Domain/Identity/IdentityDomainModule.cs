using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Hugin.Identity
{
    [DependsOn(
        typeof(AbpDddDomainModule)
    )]
    public class IdentityDomainModule : AbpModule
    {

    }
}
