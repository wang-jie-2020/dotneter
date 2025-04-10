using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nacos.V2;

namespace NacosConfiguration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ILogger<ConfigurationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public ConfigurationController(
            ILogger<ConfigurationController> logger, 
            IConfiguration configuration, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }
        
        [HttpGet]
        [Route("/get-nacos-string")]
        public object GetNacosConfigurationString()
        {
            var name = _configuration["name"];
            var email = _configuration.GetSection("email").Get<Email>();
            
            return new
            {
                name,
                email
            };
        }
        
        [HttpGet]
        [Route("/get-nacos-options")]
        
        public object GetNacosConfigurationOptions()
        {
            var options = _serviceProvider.GetService<IOptions<Email>>().Value;
            var optionsSnapshot = _serviceProvider.GetService<IOptionsSnapshot<Email>>().Value;
            var optionsMonitor = _serviceProvider.GetService<IOptionsMonitor<Email>>().CurrentValue;
        
            return new
            {
                options,
                optionsSnapshot,
                optionsMonitor
            };
        }
    }
}
