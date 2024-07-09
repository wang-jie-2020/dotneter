using AESC.Starter.Localization;

namespace AESC.Sample
{
    public abstract class SampleAppService : ApplicationService
    {
        protected SampleAppService()
        {
            LocalizationResource = typeof(StarterResource);
            ObjectMapperContext = typeof(SampleAppModule);
        }
    }
}
