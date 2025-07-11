using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Yi.AspNetCore.Auditing;
using Yi.AspNetCore.Authorization;
using Yi.AspNetCore.MultiTenancy;
using Yi.Framework.Loggings;
using Yi.System.Domains;
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

        Configure<PermissionOptions>(options =>
        {
            options.CheckHandlers.Add<UserPermissionHandler>();
        });
        
        context.Services.Replace(new ServiceDescriptor(typeof(ITenantStore), typeof(TenantStore), ServiceLifetime.Transient));
        context.Services.Replace(new ServiceDescriptor(typeof(IAuditingStore), typeof(AuditingStore), ServiceLifetime.Singleton));
        context.Services.Replace(new ServiceDescriptor(typeof(IOperLogStore), typeof(OperLogStore), ServiceLifetime.Singleton));
        
        context.Services.AddCaptcha();
    }
}