using abp_console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Volo.Abp;

var builder = Host.CreateApplicationBuilder(args);
//builder.ConfigureContainer(builder.Services.AddAutofacServiceProviderFactory());    //abp-autofac 非必须,很多项目都用的autofac且可能有aop的需要

builder.Services.AddHostedService<ConsoleAppHostedService>();

await builder.Services.AddApplicationAsync<ConsoleModule>();    //abp 需增加 RootModule

var host = builder.Build();

await host.InitializeAsync();   //abp 需增加

await host.RunAsync();