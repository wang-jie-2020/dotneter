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
        
        context.Services.AddCaptcha();
    }
}