using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Yi.Admin.Services.TenantManagement;

namespace Yi.Admin;

[DependsOn(
    typeof(AbpAuditingModule),
    typeof(AbpBackgroundWorkersQuartzModule)
)]
public class YiAdminModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Tenant
        context.Services.Replace(new ServiceDescriptor(typeof(ITenantStore), typeof(SqlSugarAndConfigurationTenantStore),
            ServiceLifetime.Transient));

        context.Services.Replace(new ServiceDescriptor(typeof(IConnectionStringResolver),
            typeof(YiMultiTenantConnectionStringResolver), ServiceLifetime.Transient));
        
        context.Services.Replace(new ServiceDescriptor(typeof(ITenantConfigurationProvider),
            typeof(YiTenantConfigurationProvider), ServiceLifetime.Transient));
    }
}