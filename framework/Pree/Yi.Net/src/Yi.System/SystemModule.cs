using Microsoft.Extensions.DependencyInjection;
using Yi.AspNetCore.Authorization;
using Yi.System.Domains;

namespace Yi.System;

[DependsOn(typeof(YiFrameworkModule))]
public class SystemModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        Configure<PermissionOptions>(options =>
        {
            options.CheckHandlers.Add<UserPermissionHandler>();
        });
        
        //context.Services.Replace(new ServiceDescriptor(typeof(ITenantStore), typeof(TenantStore), ServiceLifetime.Transient));
        //context.Services.Replace(new ServiceDescriptor(typeof(IAuditingStore), typeof(AuditingStore), ServiceLifetime.Singleton));
        //context.Services.Replace(new ServiceDescriptor(typeof(IOperLogStore), typeof(OperLogStore), ServiceLifetime.Singleton));
        
        context.Services.AddCaptcha();
    }
}