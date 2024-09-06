using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.MultiTenancy;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Infra.TenantManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(YiFrameworkDddApplicationContractsModule)
)]
public class YiFrameworkTenantManagementModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var services = context.Services;
        services.Replace(new ServiceDescriptor(typeof(ITenantStore), typeof(SqlSugarAndConfigurationTenantStore),
            ServiceLifetime.Transient));

        services.Replace(new ServiceDescriptor(typeof(IConnectionStringResolver),
            typeof(YiMultiTenantConnectionStringResolver), ServiceLifetime.Transient));
    }
}