using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AESC.Starter.Console
{
    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class StarterConsoleAppModule : AbpModule
    {
        public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var logger = context.ServiceProvider.GetRequiredService<ILogger<StarterConsoleAppModule>>();
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            var hostEnvironment = context.ServiceProvider.GetRequiredService<IHostEnvironment>();

            logger.LogInformation($"EnvironmentName => {hostEnvironment.EnvironmentName}");
            logger.LogInformation($"Connection => {configuration.GetConnectionString("Default")}");

            return Task.CompletedTask;
        }
    }

}