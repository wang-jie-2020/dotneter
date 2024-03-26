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
        typeof(AbpProCoreModule),
        typeof(SampleDomainModule)
    )]
    public class SampleAppModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(SampleAppModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<SampleAppModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<SampleAppModule>(validate: true);
            });
        }
    }
}
