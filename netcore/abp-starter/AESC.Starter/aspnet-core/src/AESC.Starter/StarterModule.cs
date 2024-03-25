using AESC.Starter.MultiTenancy;
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
        typeof(AbpObjectExtendingModule),   //todo
        typeof(AbpValidationModule),
        typeof(BasicManagementApplicationModule),
        typeof(BasicManagementApplicationContractsModule),
        typeof(BasicManagementDomainModule),
        typeof(BasicManagementDomainSharedModule),
        typeof(BasicManagementHttpApiModule),
        typeof(NotificationManagementApplicationModule),
        typeof(NotificationManagementApplicationContractsModule),
        typeof(NotificationManagementDomainModule),
        typeof(NotificationManagementDomainSharedModule),
        typeof(NotificationManagementHttpApiModule),
        typeof(DataDictionaryManagementApplicationModule),
        typeof(DataDictionaryManagementApplicationContractsModule),
        typeof(DataDictionaryManagementDomainModule),
        typeof(DataDictionaryManagementDomainSharedModule),
        typeof(DataDictionaryManagementHttpApiModule),
        typeof(LanguageManagementApplicationContractsModule),
        typeof(LanguageManagementApplicationModule),
        typeof(LanguageManagementDomainModule),
        typeof(LanguageManagementDomainSharedModule),
        typeof(LanguageManagementHttpApiModule),
        typeof(AbpProCoreModule)
    )]
    public class StarterModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(StarterModule).Assembly);
            });

            StarterDtoExtensions.Configure();
            StarterModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = MultiTenancyConsts.IsEnabled; });

            context.Services.AddAutoMapperObjectMapper<StarterModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<StarterModule>();
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<StarterModule>(StarterConsts.NameSpace);
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<StarterResource>(StarterConsts.DefaultCultureName)
                    .AddVirtualJson("/Localization/Starter")
                    .AddBaseTypes(typeof(BasicManagementResource))
                    .AddBaseTypes(typeof(NotificationManagementResource))
                    .AddBaseTypes(typeof(DataDictionaryManagementResource))
                    .AddBaseTypes(typeof(LanguageManagementResource))
                    .AddBaseTypes(typeof(AbpUiResource))
                    .AddBaseTypes(typeof(AbpTimingResource));

                options.DefaultResourceType = typeof(StarterResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace(StarterConsts.NameSpace, typeof(StarterResource));
            });
        }
    }
}
