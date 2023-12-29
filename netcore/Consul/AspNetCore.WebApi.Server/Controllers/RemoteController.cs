using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AspNetCore.WebApi.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RemoteController : ControllerBase
    {
        private readonly ILogger<RemoteController> _logger;

        public RemoteController(ILogger<RemoteController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Http远程接口
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public object Get(string name)
        {
            _logger.LogInformation($"request from {HttpContext.Connection.RemoteIpAddress},name is {name}");
            var sericeInfo = HttpContext.RequestServices.GetRequiredService<IOptions<ServiceInfo>>().Value;
            return $"data from http request=>{sericeInfo.IPAddress}:{sericeInfo.Port}";
        }
    }
}
