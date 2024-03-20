namespace AESC.Starter.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class StarterController : AbpController
    {
        protected StarterController()
        {
            LocalizationResource = typeof(StarterResource);
        }
    }
}