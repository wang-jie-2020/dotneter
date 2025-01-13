using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace abp_outer_module
{
    public class OuterModule : AbpModule
    {
        //可以类似原生方式进行注册
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            Configure<ConnOptions>(p =>
            {
                p.ConnectString = context.Services.GetConfiguration()["ConnectionStrings:Default"];
            });

            return Task.CompletedTask;
        }

        //同时项目启动时可以切入
        public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var logger = context.ServiceProvider.GetRequiredService<ILogger<OuterModule>>();
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            var hostEnvironment = context.ServiceProvider.GetRequiredService<IHostEnvironment>();
            var connOptions = context.ServiceProvider.GetRequiredService<IOptions<ConnOptions>>();

            logger.LogInformation($"EnvironmentName => {hostEnvironment.EnvironmentName}");
            logger.LogInformation($"Connection => {configuration.GetConnectionString("Default")}");

            logger.LogInformation(configuration["ANOTHER_LOCALAPP"]);
            logger.LogInformation(configuration["ASPNETCORE_LOCALAPP"]);
            logger.LogInformation(configuration["DOTNET_LOCALAPP"]);
            logger.LogInformation(configuration["LOCALAPP"]);

            return Task.CompletedTask;
        }
    }
}