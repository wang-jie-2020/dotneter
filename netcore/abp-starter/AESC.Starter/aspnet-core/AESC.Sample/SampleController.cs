using AESC.Sample.Localization;

namespace AESC.Sample
{
    public abstract class SampleController : AbpController
    {
        protected SampleController()
        {
            LocalizationResource = typeof(SampleLocalizationResource);
        }
    }
}
