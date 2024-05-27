using AESC.Starter.EntityFrameworkCore;
using AESC.Starter.Localization;
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
    public class StarterDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<StarterDomainModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<StarterDomainModule>();
            });

            // 如此注册时IRepository<Entity>中的DbContext不会指向StarterDbContext(TryAdd方法不会覆盖已有类型注册)
            // context.Services.AddAbpDbContext<StarterDbContext>(options =>
            // {
            //     options.AddDefaultRepositories(includeAllEntities: true);
            // });

            context.Services.AddAbpDbContextHybrid<StarterDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<StarterDomainModule>(StarterConsts.NameSpace);
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
