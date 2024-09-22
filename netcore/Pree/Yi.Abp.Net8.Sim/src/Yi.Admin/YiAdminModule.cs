using Yi.AspNetCore;

namespace Yi.Admin;

[DependsOn(typeof(YiAspNetCoreModule))]
public class YiAdminModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }
}