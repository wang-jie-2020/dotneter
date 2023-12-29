using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 输出配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Get()
        {
            //使用IOptionsSnapshot<>，它是Scoped作用域
            var optionsSnapshot = HttpContext.RequestServices.GetService<IOptionsSnapshot<TestOptions>>();

            //因为IOptionsMonitor<>有缓存IOptionsMonitorCache<>，所以需要注意使用
            //如果使用service.Configure<TOptions>(IConfiguration)方法添加的Options，否则需要自行设置重新加载机制
            //重新加载可参考：https://www.cnblogs.com/shanfeng1000/p/15095236.html
            var optionsMonitor = HttpContext.RequestServices.GetService<IOptionsMonitor<TestOptions>>();

            return new
            {
                OptionsSnapshot = optionsSnapshot.Value,
                OptionsMonitor = optionsMonitor.CurrentValue
            };
        }
    }
}
