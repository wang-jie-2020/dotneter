using Demo.Aop;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    /*
     *  Controller中不属于动态代理实现AOP的范围,必须通过IFilterMetadata
     *  尝试了此处和abp处，大概也有一点认知，这块有时间可以深入理解一下，有好处
     *
     */
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
