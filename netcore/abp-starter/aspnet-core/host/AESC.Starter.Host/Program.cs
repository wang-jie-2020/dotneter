using NLog.Web;

namespace AESC.Starter.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) => { options.Limits.MaxRequestBodySize = 1024 * 50; });
                    webBuilder.UseStartup<Startup>();
                })
                .UseNLog()
                .UseAutofac();
    }
}
