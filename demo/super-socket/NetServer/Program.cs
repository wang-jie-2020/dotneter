using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetServer.Server;

namespace NetServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            var host = builder.ConfigureServices(services =>
            {
                services.AddHostedService<ServerHostedService>();
                services.AddSingleton<SocketServerInitializer>();
                services.Configure<SocketServerOptions>(options => { });
            })
            .UseConsoleLifetime()
            .Build();

            host.Run();
        }
    }
}