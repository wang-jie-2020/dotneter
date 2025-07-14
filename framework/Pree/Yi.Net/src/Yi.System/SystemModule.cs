using Microsoft.Extensions.DependencyInjection;

namespace Yi.System;

[DependsOn(typeof(YiFrameworkModule))]
public class SystemModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }
}