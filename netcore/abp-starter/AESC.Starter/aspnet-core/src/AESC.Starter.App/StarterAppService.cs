using AESC.Starter.Localization;

namespace AESC.Starter
{
    public abstract class StarterAppService : ApplicationService
    {
        protected StarterAppService()
        {
            LocalizationResource = typeof(StarterResource);
        }
    }
}
