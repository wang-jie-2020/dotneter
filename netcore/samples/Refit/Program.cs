using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

var builder = Host.CreateApplicationBuilder(args);
builder.ConfigureContainer(builder.Services.AddAutofacServiceProviderFactory());  
await builder.Services.AddApplicationAsync<MainModule>();

var app = builder.Build();
await app.InitializeAsync();
app.Run();

[DependsOn(typeof(AbpAutofacModule))]
internal class MainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // var gitHubApi = RestService.For<IGitHubApi>("https://api.github.com");
        // var octocat = await gitHubApi.GetUser("octocat");

        // context.Services
        //     .AddRefitClient<IGitHubApi>()
        //     .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.github.com"));

        //var octocat = await gitHubApi.GetUser("octocat");
        // builder.Services.AddRefitClient<IGitHubApi>().ConfigurePrimaryHttpMessageHandler<NacosDiscoveryHttpClientHandler>();
    }
    
    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var sc = context.ServiceProvider.GetService<IGitHubApi>();
        await sc.GetUser("octocat");
    }
}