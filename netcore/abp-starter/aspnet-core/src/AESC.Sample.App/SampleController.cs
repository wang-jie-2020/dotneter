using AESC.Shared.Localization;

namespace AESC.Sample
{
    public abstract class SampleController : AbpController
    {
        protected SampleController()
        {
            LocalizationResource = typeof(SharedLocalizationResource);
        }
    }
}
