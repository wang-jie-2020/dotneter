using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Auditing;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.AuditLogging;
using Yi.Infra.AuditLogging.Repositories;
using Yi.Infra.Rbac;
using Yi.Infra.Rbac.Authorization;
using Yi.Infra.Rbac.Operlog;
using Yi.Infra.Rbac.Options;
using Yi.Infra.TenantManagement;

namespace Yi.Infra;

[DependsOn(
    typeof(AbpAuditingModule),
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpBackgroundWorkersQuartzModule)
)]
public class YiInfraModule : AbpModule
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
        
        context.Services.TryAddYiDbContext<YiRbacDbContext>();
        
        var service = context.Services;

        service.AddCaptcha();

        var configuration = context.Services.GetConfiguration();
        service.AddControllers(options =>
        {
            options.Filters.Add<PermissionGlobalAttribute>();
            options.Filters.Add<OperLogGlobalAttribute>();
        });

        //配置阿里云短信
        Configure<AliyunOptions>(configuration.GetSection(nameof(AliyunOptions)));

        Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        Configure<RefreshJwtOptions>(configuration.GetSection(nameof(RefreshJwtOptions)));
        Configure<RbacOptions>(configuration.GetSection(nameof(RbacOptions)));

        context.Services.TryAddYiDbContext<YiRbacDbContext>();
    }
}