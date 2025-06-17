using Volo.Abp.Autofac;
using Volo.Abp.Uow;

namespace Yi.AspNetCore;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpUnitOfWorkModule),
    typeof(YiFrameworkModule)
)]
public class YiAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {

    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
       
    }
}