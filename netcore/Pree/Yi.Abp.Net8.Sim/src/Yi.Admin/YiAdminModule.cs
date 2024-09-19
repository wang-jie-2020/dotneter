using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Yi.Admin.Domains.AuditLogging;
using Yi.Admin.Domains.AuditLogging.Repositories;
using Yi.Admin.Services.TenantManagement;
using Yi.AspNetCore;

namespace Yi.Admin;

[DependsOn(typeof(YiAspNetCoreModule))]
public class YiAdminModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Auditing
        context.Services.AddTransient<IAuditingStore, AuditingStore>();
        context.Services.AddTransient<IAuditLogRepository, SqlSugarCoreAuditLogRepository>();
        context.Services.AddTransient<IAuditLogInfoToAuditLogConverter, AuditLogInfoToAuditLogConverter>();

        //Tenant
        context.Services.Replace(new ServiceDescriptor(typeof(ITenantStore), typeof(SqlSugarAndConfigurationTenantStore),
            ServiceLifetime.Transient));

        context.Services.Replace(new ServiceDescriptor(typeof(IConnectionStringResolver),
            typeof(YiMultiTenantConnectionStringResolver), ServiceLifetime.Transient));

        context.Services.Replace(new ServiceDescriptor(typeof(ITenantConfigurationProvider),
            typeof(YiTenantConfigurationProvider), ServiceLifetime.Transient));
    }
}