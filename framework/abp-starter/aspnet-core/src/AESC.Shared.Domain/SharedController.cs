using AESC.Shared.Localization;

namespace AESC.Shared
{
    public abstract class SharedController : AbpController
    {
        protected SharedController()
        {
            LocalizationResource = typeof(SharedLocalizationResource);
        }
    }
}