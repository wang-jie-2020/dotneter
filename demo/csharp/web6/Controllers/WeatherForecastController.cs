using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;

namespace web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var file1 = "http://localhost:5138/G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127.pdf"; //404
            var file2 = "http://localhost:5138/G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127.pdf"; //404

            var file3 = $"http://localhost:5138/{System.Net.WebUtility.UrlEncode("G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127.pdf")}"; //404
            var file4 = $"http://localhost:5138/{System.Net.WebUtility.UrlEncode("G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127.pdf")}"; //404

            var file5 = $"http://localhost:5138/{HttpUtility.UrlEncode("G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127.pdf", Encoding.UTF8)}"; //404
            var file6 = $"http://localhost:5138/{HttpUtility.UrlEncode("G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127.pdf", Encoding.UTF8)}"; //404


            var file7 = $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127.pdf"; //200
            var file8 = $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127.pdf"; //404

            var file9 = $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127 - 副本.pdf"; //200
            var file10 = $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127 - 副本.pdf"; //404

            var file11 = $"https://minio.envision-aesc.cn:6443/galileo-test/{System.Net.WebUtility.UrlEncode("G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127.pdf")}"; //404
            var file12 = $"https://minio.envision-aesc.cn:6443/galileo-test/{System.Net.WebUtility.UrlEncode("G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127.pdf")}"; //404

            var file13 = $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL%.pdf";
            var file14 = $"https://minio.envision-aesc.cn:6443/galileo-test/{System.Net.WebUtility.UrlEncode("G06254_EOL%.pdf")}";

            var file15 = $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL#.pdf";
            var file16 = $"https://minio.envision-aesc.cn:6443/galileo-test/{System.Net.WebUtility.UrlEncode("G06254_EOL#.pdf")}";

            var httClient = new HttpClient();

            try
            {
                Console.WriteLine(file1);
                var stream = httClient.GetStreamAsync(file1).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file2);
                var stream = httClient.GetStreamAsync(file2).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file3);
                var stream = httClient.GetStreamAsync(file3).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file4);
                var stream = httClient.GetStreamAsync(file4).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file5);
                var stream = httClient.GetStreamAsync(file5).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file6);
                var stream = httClient.GetStreamAsync(file6).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file7);
                var stream = httClient.GetStreamAsync(file7).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file8);
                var stream = httClient.GetStreamAsync(file8).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file9);
                var stream = httClient.GetStreamAsync(file9).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file10);
                var stream = httClient.GetStreamAsync(file10).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file11);
                var stream = httClient.GetStreamAsync(file11).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file12);
                var stream = httClient.GetStreamAsync(file12).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file13);
                var stream = httClient.GetStreamAsync(file13).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file14);
                var stream = httClient.GetStreamAsync(file14).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file15);
                var stream = httClient.GetStreamAsync(file15).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            try
            {
                Console.WriteLine(file16);
                var stream = httClient.GetStreamAsync(file16).Result;
            }
            catch
            {
                Console.WriteLine("1");
            }

            return Enumerable.Range(1, 5)
                .Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpGet]
        [Route("sln")]
        public async Task HowTo()
        {
            var urls = new[]
            {
                $"https://minio.envision-aesc.cn:6443/galileo-test/G&0=6#25 4_EO+L%.pdf",
                $"https://minio.envision-aesc.cn:6443/galileo-test/{HttpUtility.UrlEncode("G&0=6#25 4_EO+L%.pdf")}",
                $"https://minio.envision-aesc.cn:6443/galileo-test/{HttpUtility.UrlEncode("G&0=6#25 4_EO+L%.pdf").Replace("+", "%20")}",
            };

            var httClient = new HttpClient();

            foreach (var url in urls)
            {
                try
                {
                    var stream = await httClient.GetStreamAsync(url);
                    Console.WriteLine($"Url: {url} --- Result:Success");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Url: {url} --- Result:Failed");
                }
            }
        }


        [HttpGet]
        [Route("GerMinioUrls")]
        public async Task GetMinioUrls()
        {
            var urls = new[]
            {
                // $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL%.pdf",    //200
                // $"https://minio.envision-aesc.cn:6443/galileo-test/{WebUtility.UrlEncode("G06254_EOL%.pdf")}", //200
                //
                // $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL#.pdf", //404
                // $"https://minio.envision-aesc.cn:6443/galileo-test/{WebUtility.UrlEncode("G06254_EOL#.pdf")}", //200

                $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127.pdf", //200
                $"https://minio.envision-aesc.cn:6443/galileo-test/G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127.pdf", //404

                $"https://minio.envision-aesc.cn:6443/galileo-test/{WebUtility.UrlEncode("G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127.pdf")}", //404
                $"https://minio.envision-aesc.cn:6443/galileo-test/{WebUtility.UrlEncode("G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127.pdf")}", //404

                $"https://minio.envision-aesc.cn:6443/galileo-test/{WebUtility.UrlEncode("G06254_EOL测试数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127.pdf")}", //200
                $"https://minio.envision-aesc.cn:6443/galileo-test/{WebUtility.UrlEncode("G06254_EOL测试数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127.pdf")}", //200

                $"https://minio.envision-aesc.cn:6443/galileo-test/{"G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74%_孙文龙_20241127.pdf".Replace("%", "%25").Replace("#", "%23").Replace(" ", "%20")}", //200
                $"https://minio.envision-aesc.cn:6443/galileo-test/{"G06254_EOL测试 数据采集仪_电芯电压6_交叉型GRR_2.74#_孙文龙_20241127.pdf".Replace("%", "%25").Replace("#", "%23").Replace(" ", "%20")}", //200
            };

            var httClient = new HttpClient();

            foreach (var url in urls)
            {
                try
                {
                    var stream = await httClient.GetStreamAsync(url);
                    Console.WriteLine($"Url: {url} --- Result:Success");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Url: {url} --- Result:Failed");
                }
            }
        }
    }
}