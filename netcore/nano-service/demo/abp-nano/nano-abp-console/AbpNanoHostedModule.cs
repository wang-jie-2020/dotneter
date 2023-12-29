using Demo.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Demo;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(HelloModule),
    typeof(DemoEntityFrameworkCoreModule)
)]
public class AbpNanoHostedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostingEnvironment = context.Services.GetSingletonInstance<IHostEnvironment>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.UseSqlServer();
        });
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var logger = context.ServiceProvider.GetRequiredService<ILogger<AbpNanoHostedModule>>();
        var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
        var hostEnvironment = context.ServiceProvider.GetRequiredService<IHostEnvironment>();

        return Task.CompletedTask;
    }
}