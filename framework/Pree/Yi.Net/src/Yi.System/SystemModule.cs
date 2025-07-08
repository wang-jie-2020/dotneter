using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Yi.AspNetCore.MultiTenancy;
using Yi.Framework.Auditing;
using Yi.Framework.Loggings;
using Yi.Framework.Permissions;
using Yi.System.Domains;
using Yi.System.Monitor;
using Yi.System.Monitor.Repositories;
using Yi.System.Options;

namespace Yi.System;

[DependsOn(typeof(YiFrameworkModule))]
public class SystemModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        Configure<RefreshJwtOptions>(configuration.GetSection(nameof(RefreshJwtOptions)));
        Configure<RbacOptions>(configuration.GetSection(nameof(RbacOptions)));
        
        //Tenant
        context.Services.Replace(new ServiceDescriptor(typeof(ITenantStore), typeof(TenantStore), ServiceLifetime.Transient));
        
        //System
        context.Services.Replace(new ServiceDescriptor(typeof(IAuditingStore), typeof(AuditingStore), ServiceLifetime.Singleton));
        context.Services.Replace(new ServiceDescriptor(typeof(IOperLogStore), typeof(OperLogStore), ServiceLifetime.Singleton));
        context.Services.Replace(new ServiceDescriptor(typeof(IPermissionHandler), typeof(UserPermissionHandler), ServiceLifetime.Transient));
        context.Services.AddTransient<IAuditLogRepository, SqlSugarCoreAuditLogRepository>();
        context.Services.AddTransient<IAuditLogInfoToAuditLogConverter, AuditLogInfoToAuditLogConverter>();
        
        context.Services.AddCaptcha();
    }
}