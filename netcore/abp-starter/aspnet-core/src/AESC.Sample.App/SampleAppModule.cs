using AESC.Shared;
using Localization.Resources.AbpUi;

namespace AESC.Sample
{
    [DependsOn(
        typeof(SharedDomainModule)
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
