using AESC.Shared.Localization;

namespace AESC.Starter
{
    public abstract class StarterController : AbpController
    {
        protected StarterController()
        {
            LocalizationResource = typeof(SharedLocalizationResource);
        }
    }
}