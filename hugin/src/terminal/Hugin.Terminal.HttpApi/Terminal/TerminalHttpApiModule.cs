using Hugin.Terminal.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Hugin.Terminal
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(TerminalApplicationContractsModule)
        )]
    public class TerminalHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(TerminalHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<TerminalResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
