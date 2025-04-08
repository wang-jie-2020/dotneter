using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RedisGEO.Core;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.UseGeoRedis("127.0.0.1");

            var sp = services.BuildServiceProvider();

            var options = sp.GetRequiredService<IOptions<GeoConfig>>();
            var service = sp.GetRequiredService<IGeoService>();

            Debug.Assert(options == null);
            Debug.Assert(service == null);
        }
    }
}
