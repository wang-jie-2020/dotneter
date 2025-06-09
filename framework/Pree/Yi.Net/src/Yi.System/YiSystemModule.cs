using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Yi.AspNetCore;
using Yi.AspNetCore.Core.Loggings;
using Yi.AspNetCore.Core.Permissions;
using Yi.Sys.Domains.Infra;
using Yi.Sys.Domains.Monitor;
using Yi.Sys.Domains.Monitor.Repositories;
using Yi.Sys.Options;

namespace Yi.Sys;

[DependsOn(typeof(YiAspNetCoreModule))]
public class YiSystemModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        Configure<RefreshJwtOptions>(configuration.GetSection(nameof(RefreshJwtOptions)));
        Configure<RbacOptions>(configuration.GetSection(nameof(RbacOptions)));
        
        //Tenant
        context.Services.Replace(new ServiceDescriptor(typeof(ITenantStore), typeof(SqlSugarAndConfigurationTenantStore), ServiceLifetime.Transient));
        context.Services.Replace(new ServiceDescriptor(typeof(IConnectionStringResolver), typeof(YiMultiTenantConnectionStringResolver), ServiceLifetime.Transient));
        context.Services.Replace(new ServiceDescriptor(typeof(ITenantConfigurationProvider), typeof(YiTenantConfigurationProvider), ServiceLifetime.Transient));
        
        //System
        context.Services.Replace(new ServiceDescriptor(typeof(IAuditingStore), typeof(AuditingStore), ServiceLifetime.Singleton));
        context.Services.Replace(new ServiceDescriptor(typeof(IOperLogStore), typeof(OperLogStore), ServiceLifetime.Singleton));
        context.Services.Replace(new ServiceDescriptor(typeof(IPermissionHandler), typeof(UserPermissionHandler), ServiceLifetime.Transient));
        context.Services.AddTransient<IAuditLogRepository, SqlSugarCoreAuditLogRepository>();
        context.Services.AddTransient<IAuditLogInfoToAuditLogConverter, AuditLogInfoToAuditLogConverter>();
        
        context.Services.AddCaptcha();
        context.Services.TryAddYiDbContext<YiDataScopedDbContext>();
    }
}