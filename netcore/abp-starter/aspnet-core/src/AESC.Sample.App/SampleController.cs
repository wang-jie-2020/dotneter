using AESC.Starter.Localization;

namespace AESC.Sample
{
    public abstract class SampleController : AbpController
    {
        protected SampleController()
        {
            LocalizationResource = typeof(StarterResource);
        }
    }
}
