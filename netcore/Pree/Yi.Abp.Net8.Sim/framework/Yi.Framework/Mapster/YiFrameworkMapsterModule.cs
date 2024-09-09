using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.ObjectMapping;
using Yi.Framework.Core;

namespace Yi.Framework.Mapster;

[DependsOn(typeof(AbpObjectMappingModule))]
public class YiFrameworkMapsterModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IAutoObjectMappingProvider, MapsterAutoObjectMappingProvider>();
    }
}