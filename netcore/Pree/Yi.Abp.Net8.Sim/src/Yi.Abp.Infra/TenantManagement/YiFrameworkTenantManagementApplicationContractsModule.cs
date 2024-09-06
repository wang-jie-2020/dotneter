using Volo.Abp.TenantManagement;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.TenantManagement
{
    [DependsOn(typeof(AbpTenantManagementDomainSharedModule),
        typeof(YiFrameworkDddApplicationContractsModule))]
    public class YiFrameworkTenantManagementApplicationContractsModule:AbpModule
    {

    }
}
