using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac
{
    [DependsOn(
        typeof(YiFrameworkRbacDomainSharedModule),


        typeof(YiFrameworkDddApplicationContractsModule))]
    public class YiFrameworkRbacApplicationContractsModule : AbpModule
    {

    }
}