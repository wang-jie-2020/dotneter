using AESC.Shared.EntityFrameworkCore;
using AESC.Shared.Localization;
using AESC.Utils.AbpExtensions.EntityFrameworkCore;
using Lion.AbpPro.BasicManagement;
using Lion.AbpPro.BasicManagement.EntityFrameworkCore;
using Lion.AbpPro.BasicManagement.Localization;
using Lion.AbpPro.DataDictionaryManagement;
using Lion.AbpPro.DataDictionaryManagement.EntityFrameworkCore;
using Lion.AbpPro.DataDictionaryManagement.Localization;
using Lion.AbpPro.LanguageManagement;
using Lion.AbpPro.LanguageManagement.EntityFrameworkCore;
using Lion.AbpPro.LanguageManagement.Localization;
using Lion.AbpPro.NotificationManagement;
using Lion.AbpPro.NotificationManagement.EntityFrameworkCore;
using Lion.AbpPro.NotificationManagement.Localization;
using Localization.Resources.AbpUi;

namespace AESC.Shared
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
        typeof(BasicManagementDomainModule),
        typeof(BasicManagementDomainSharedModule),
        typeof(BasicManagementEntityFrameworkCoreModule),
        typeof(NotificationManagementDomainModule),
        typeof(NotificationManagementDomainSharedModule),
        typeof(NotificationManagementEntityFrameworkCoreModule),
        typeof(DataDictionaryManagementDomainModule),
        typeof(DataDictionaryManagementDomainSharedModule),
        typeof(DataDictionaryManagementEntityFrameworkCoreModule),
        typeof(LanguageManagementDomainModule),
        typeof(LanguageManagementDomainSharedModule),
        typeof(LanguageManagementEntityFrameworkCoreModule),
        typeof(AbpProCoreModule)
    )]
    public class SharedDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SharedDomainModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SharedDomainModule>();
            });

            context.Services.AddAbpDbContextHybrid<SharedDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SharedDomainModule>(SharedConsts.NameSpace);
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<SharedLocalizationResource>(SharedConsts.DefaultCultureName)
                    .AddVirtualJson("/Localization/Starter")
                    .AddBaseTypes(typeof(BasicManagementResource))
                    .AddBaseTypes(typeof(NotificationManagementResource))
                    .AddBaseTypes(typeof(DataDictionaryManagementResource))
                    .AddBaseTypes(typeof(LanguageManagementResource))
                    .AddBaseTypes(typeof(AbpUiResource))
                    .AddBaseTypes(typeof(AbpTimingResource));

                options.DefaultResourceType = typeof(SharedLocalizationResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace(SharedConsts.NameSpace, typeof(SharedLocalizationResource));
            });
        }
    }
}
