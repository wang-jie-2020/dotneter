using AESC.Sample.EntityFrameworkCore;
using AESC.Sample.Localization;
using AESC.Utils.AbpExtensions.EntityFrameworkCore;
using Localization.Resources.AbpUi;

namespace AESC.Sample
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
        typeof(AbpProCoreModule)
    )]
    public class SampleDomainModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SampleDomainModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SampleDomainModule>(validate: true);
            });

            context.Services.AddAbpDbContext<SampleDbContext>(options =>
            {
                EntityFrameworkCoreRepositoriesUtils.AddConfiguredTypeRepository<SampleDbContext>(options);
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SampleDomainModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<SampleLocalizationResource>(SampleConsts.DefaultCultureName)
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddBaseTypes(typeof(AbpUiResource))
                    .AddVirtualJson("/Localization/Sample");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace(SampleConsts.NameSpace, typeof(SampleLocalizationResource));
            });
        }
    }
}
