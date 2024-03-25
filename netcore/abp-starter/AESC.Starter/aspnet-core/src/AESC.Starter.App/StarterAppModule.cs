using AESC.Starter.EntityFrameworkCore;
using AESC.Starter.Localization;
using Lion.AbpPro.BasicManagement;
using Lion.AbpPro.BasicManagement.Localization;
using Lion.AbpPro.DataDictionaryManagement;
using Lion.AbpPro.DataDictionaryManagement.Localization;
using Lion.AbpPro.LanguageManagement;
using Lion.AbpPro.LanguageManagement.Localization;
using Lion.AbpPro.NotificationManagement;
using Lion.AbpPro.NotificationManagement.Localization;
using Localization.Resources.AbpUi;

namespace AESC.Starter
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpCachingModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpDddDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpValidationModule),
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
        typeof(AbpProCoreModule),
        typeof(StarterDomainModule)
    )]
    public class StarterAppModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(StarterAppModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<StarterAppModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<StarterAppModule>();
            });
        }
    }
}
