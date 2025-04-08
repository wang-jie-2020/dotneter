using Lion.AbpPro.BasicManagement;
using Lion.AbpPro.DataDictionaryManagement;
using Lion.AbpPro.LanguageManagement;
using Lion.AbpPro.NotificationManagement;

namespace AESC.Shared
{
    [DependsOn(
        typeof(BasicManagementApplicationModule),
        typeof(BasicManagementApplicationContractsModule),
        typeof(BasicManagementHttpApiModule),
        typeof(NotificationManagementApplicationModule),
        typeof(NotificationManagementApplicationContractsModule),
        typeof(NotificationManagementHttpApiModule),
        typeof(DataDictionaryManagementApplicationModule),
        typeof(DataDictionaryManagementApplicationContractsModule),
        typeof(DataDictionaryManagementHttpApiModule),
        typeof(LanguageManagementApplicationContractsModule),
        typeof(LanguageManagementApplicationModule),
        typeof(LanguageManagementHttpApiModule),
        typeof(SharedDomainModule)
    )]
    public class SharedAppModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SharedAppModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SharedAppModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SharedAppModule>();
            });
        }
    }
}
