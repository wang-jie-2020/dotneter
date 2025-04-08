using Hugin.Platform.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Hugin.Platform
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(PlatformApplicationContractsModule)
        )]
    public class PlatformHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(PlatformHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PlatformResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
