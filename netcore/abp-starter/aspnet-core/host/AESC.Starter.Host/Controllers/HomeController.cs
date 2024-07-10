namespace AESC.Starter.Host.Controllers
{
    public class HomeController : AbpController
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public ActionResult Index()
        {
            logger.LogDebug("debug");
            logger.LogInformation("info");
            logger.LogWarning("warn");
            logger.LogError("error");
            logger.LogCritical("critical");

            return Redirect("/Login");
        }
    }
}
