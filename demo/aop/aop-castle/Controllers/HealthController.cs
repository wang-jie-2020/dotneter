using Autofac.Extras.DynamicProxy;
using Demo.Aop;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public virtual object Index()
        {
            return "Healthy";
        }
    }
}
