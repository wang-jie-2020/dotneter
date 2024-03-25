using AESC.Starter.Localization;

namespace AESC.Starter
{
    public abstract class StarterController : AbpController
    {
        protected StarterController()
        {
            LocalizationResource = typeof(StarterResource);
        }
    }
}