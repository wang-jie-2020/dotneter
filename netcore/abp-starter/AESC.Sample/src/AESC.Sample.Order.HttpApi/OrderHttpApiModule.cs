using Localization.Resources.AbpUi;
using AESC.Sample.Order.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace AESC.Sample.Order
{
    [DependsOn(
        typeof(OrderApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class OrderHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(OrderHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<OrderResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
