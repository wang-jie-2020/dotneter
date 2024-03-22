using AESC.Sample.EntityFrameworkCore;
using AESC.Sample.Localization;
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
    public class SampleModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SampleModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SampleModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SampleModule>(validate: true);
            });

            context.Services.AddAbpDbContext<SampleDbContext>(options =>
            {

            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SampleModule>();
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
