using Nacos.AspNetCore.V2;
using Nacos.Extensions.DiscoveryHandler;
using Nacos.V2;
using Refit;
using RefitDiscovery.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNacosAspNet(builder.Configuration, section: "Nacos");

builder.Services
    .AddRefitClient<UseRefitController.IGitHubApi2>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.github.com"));

// new Uri(builder.Configuration.GetSection("Nacos:ServerAddresses").Get<string[]>()[0]
builder.Services
    .AddRefitClient<UseRefitController.IWeatherForecast>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://app"))
    .ConfigurePrimaryHttpMessageHandler(provider =>
    {
        var svc = provider.GetRequiredService<INacosNamingService>();
        var loggerFactory = provider.GetService<ILoggerFactory>();
        return new NacosDiscoveryHandler(svc, "DEFAULT_GROUP", "DEFAULT", loggerFactory);
    });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();