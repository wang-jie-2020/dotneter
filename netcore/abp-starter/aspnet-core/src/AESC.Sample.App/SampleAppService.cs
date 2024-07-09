using AESC.Shared.Localization;

namespace AESC.Sample
{
    public abstract class SampleAppService : ApplicationService
    {
        protected SampleAppService()
        {
            LocalizationResource = typeof(SharedLocalizationResource);
            ObjectMapperContext = typeof(SampleAppModule);
        }
    }
}
