using AbpConsole.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Modularity;

var builder = Host.CreateApplicationBuilder(args);
builder.ConfigureContainer(builder.Services.AddAutofacServiceProviderFactory());  
await builder.Services.AddApplicationAsync<MainModule>();  

var host = builder.Build();
await host.InitializeAsync();  
await host.RunAsync();

// [DependsOn(typeof(AbpAutofacModule))]
internal class MainModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(c =>
        {
            if (typeof(SimpleInterceptionTargetClass) == c.ImplementationType)
            {
                c.Interceptors.Add<SimpleInterceptor>();
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // DependsOn 也可
        context.Services.AddTransient(typeof(AbpAsyncDeterminationInterceptor<>));
        
        context.Services.AddTransient<SimpleInterceptor>();
        context.Services.AddTransient<SimpleInterceptionTargetClass>();
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var sc = context.ServiceProvider.GetRequiredService<SimpleInterceptionTargetClass>();
        sc.DoIt();
        
        return Task.CompletedTask;
    }
}