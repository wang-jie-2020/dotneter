using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using pythoncsharp;
using Volo.Abp;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix: "EDA_");

await builder.Services.AddApplicationAsync<CoreModule>();    

var host = builder.Build();

await host.InitializeAsync();  

await host.RunAsync();