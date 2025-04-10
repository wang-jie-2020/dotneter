using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nacos.AspNetCore.V2;
using Nacos.V2;

namespace NacosDiscovery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscoveryController : ControllerBase
    {
        private readonly INacosNamingService _nacosNamingService;
        private readonly NacosAspNetOptions _options;

        public DiscoveryController(INacosNamingService nacosNamingService,IOptions<NacosAspNetOptions> options)
        {
            _nacosNamingService = nacosNamingService;
            _options = options.Value;
        }
        
        [HttpGet]
        [Route("/get-nacos-instance")]

        public async Task<object> GetInstance()
        {
            // 被调用方的服务名称
            var instance = await _nacosNamingService.SelectOneHealthyInstance(_options.ServiceName);
            var host = $"{instance.Ip}:{instance.Port}";

            var baseUrl = instance.Metadata.TryGetValue("secure", out _)
                ? $"https://{host}"
                : $"http://{host}";
            
            return baseUrl;
        }
    }
}
