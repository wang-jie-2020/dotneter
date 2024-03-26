using AESC.Sample.Localization;

namespace AESC.Sample
{
    public abstract class SampleAppService : ApplicationService
    {
        protected SampleAppService()
        {
            LocalizationResource = typeof(SampleLocalizationResource);
            ObjectMapperContext = typeof(SampleAppModule);
        }
    }
}
