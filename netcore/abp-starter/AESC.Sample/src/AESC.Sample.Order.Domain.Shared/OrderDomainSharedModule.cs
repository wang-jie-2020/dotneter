using Lion.AbpPro.Core;

namespace AESC.Sample.Order
{
    [DependsOn(
        typeof(AbpValidationModule),
        typeof(AbpProCoreModule)
    )]
    public class OrderDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<OrderDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<OrderResource>(OrderConsts.DefaultCultureName)
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/Order");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace(OrderConsts.NameSpace, typeof(OrderResource));
            });
        }
    }
}
