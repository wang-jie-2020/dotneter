using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nacos.V2;

namespace Usage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly INacosNamingService _nacosNamingService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        //public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration, INacosNamingService nacosNamingService)
        //{
        //    _logger = logger;
        //    _configuration = configuration;
        //    _nacosNamingService = nacosNamingService;
        //}

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("/get-nacos-all")]

        public object GetNacosConfiguation()
        {
            var ver = _configuration["Version"];

            return ver ?? "empty" ;
        }



        [HttpGet]
        [Route("/get-nacos-config")]

        public object GetConfiguation()
        {
            var a = _configuration["RedisConn"];
            var b = _configuration["tech"];


            return new[]
            {
                _configuration["RedisConn"] ?? "cannot get",
                _configuration["tech"] ?? "cannot get"
            };
        }

        [HttpGet]
        [Route("/get-nacos-options")]

        public object GetOptions()
        {
            var basicWay = _configuration.GetSection("Email").Get<Email>();

            var optionsWay = _serviceProvider.GetService<IOptions<Email>>().Value;
            var optionsSnapshotWay = _serviceProvider.GetService<IOptionsSnapshot<Email>>().Value;
            var optionsMonitorWay = _serviceProvider.GetService<IOptionsMonitor<Email>>().CurrentValue;

            return new
            {
                basicWay = basicWay,
                optionsWay = optionsWay,
                optionsSnapshotWay = optionsSnapshotWay,
                optionsMonitorWay = optionsMonitorWay
            };
        }

        [HttpGet]
        [Route("/get-nacos-instance")]

        public async Task<object> GetInstance()
        {
            // 被调用方的服务名称
            var instance = await _nacosNamingService.SelectOneHealthyInstance("dotnet-nacos-service");
            var host = $"{instance.Ip}:{instance.Port}";

            var baseUrl = instance.Metadata.TryGetValue("secure", out _)
                ? $"https://{host}"
                : $"http://{host}";

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return "empty";
            }

            return baseUrl;

            //var url = $"{baseUrl}/api/values";

            //using (HttpClient client = new HttpClient())
            //{
            //    var result = await client.GetAsync(url);
            //    return await result.Content.ReadAsStringAsync();
            //}
        }
    }
}
