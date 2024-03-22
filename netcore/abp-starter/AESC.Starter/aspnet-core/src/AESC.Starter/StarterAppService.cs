namespace AESC.Starter
{
    /* Inherit your application services from this class.
     */
    public abstract class StarterAppService : ApplicationService
    {
        protected StarterAppService()
        {
            LocalizationResource = typeof(StarterResource);
        }
    }
}
