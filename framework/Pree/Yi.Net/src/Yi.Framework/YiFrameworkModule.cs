using Volo.Abp.Autofac;
using Volo.Abp.Uow;

namespace Yi.Framework;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpUnitOfWorkModule)
)]
public class YiFrameworkModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }
}