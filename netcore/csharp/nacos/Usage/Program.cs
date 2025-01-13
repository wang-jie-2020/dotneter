using Microsoft.Extensions.Configuration;
using Nacos.V2;
using Usage;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Configuration.AddNacosV2Configuration(builder.Configuration.GetSection("Nacos"));


if (builder.Configuration["Profile"] == "test")
{
    builder.Configuration.AddNacosV2Configuration(builder.Configuration.GetSection("Nacos_test"));
}
else
{
    builder.Configuration.AddNacosV2Configuration(builder.Configuration.GetSection("Nacos_dev"));
}

builder.Services.Configure<Email>(builder.Configuration.GetSection("Email"));
// 等效于
//builder.Services.Configure<Email>(a =>
//{
//    a.Address = builder.Configuration["Email:Address"];
//    a.Name = builder.Configuration["Email:Name"];
//});



//通常只会要求读Nacos,而不会要求写Nacos,这里就足够
//builder.Services.AddNacosAspNet(builder.Configuration, section: "Nacos"); //服务发现部分

////有必要对nacos crud时才会用到这里
//builder.Services.AddNacosV2Config(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


//using (var scope = app.Services.CreateScope())
//{
//    var dataId = "dynamic";
//    var group = "inside";

//    var configSvc = scope.ServiceProvider.GetService<INacosConfigService>();

//    var content = await configSvc.GetConfig(dataId, group, 3000);
//    Console.WriteLine(content);

//    var listener = new ConfigListener();
//    await configSvc.AddListener(dataId, group, listener);

//    var isPublishOk = await configSvc.PublishConfig(dataId, group, "content");
//    Console.WriteLine(isPublishOk);

//    await Task.Delay(3000);
//    content = await configSvc.GetConfig(dataId, group, 5000);
//    Console.WriteLine(content);

//    var isRemoveOk = await configSvc.RemoveConfig(dataId, group);
//    Console.WriteLine(isRemoveOk);
//    await Task.Delay(3000);

//    content = await configSvc.GetConfig(dataId, group, 5000);
//    Console.WriteLine(content);
//}

app.Run();


internal class ConfigListener : IListener
{
    public void ReceiveConfigInfo(string configInfo)
    {
        Console.WriteLine("recieve:" + configInfo);
    }
}


///*
//Demo for Basic Nacos Opreation
//App.csproj

//<ItemGroup>
//  <PackageReference Include="nacos-sdk-csharp" Version="${latest.version}" />
//</ItemGroup>
//*/

//using Microsoft.Extensions.DependencyInjection;
//using Nacos.V2;
//using Nacos.V2.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//class Program
//{
//    static async Task Main(string[] args)
//    {
//        string serverAddr = "http://localhost:8848";
//        string dataId = "demo.default";
//        string group = "demo";

//        IServiceCollection services = new ServiceCollection();

//        services.AddNacosV2Config(x =>
//        {
//            x.ServerAddresses = new List<string> { serverAddr };
//            x.Namespace = "cs-test";

//            // swich to use http or rpc
//            x.ConfigUseRpc = true;
//        });

//        IServiceProvider serviceProvider = services.BuildServiceProvider();
//        var configSvc = serviceProvider.GetService<INacosConfigService>();

//        var content = await configSvc.GetConfig(dataId, group, 3000);
//        Console.WriteLine(content);

//        var listener = new ConfigListener();

//        await configSvc.AddListener(dataId, group, listener);

//        var isPublishOk = await configSvc.PublishConfig(dataId, group, "content");
//        Console.WriteLine(isPublishOk);

//        await Task.Delay(3000);
//        content = await configSvc.GetConfig(dataId, group, 5000);
//        Console.WriteLine(content);

//        var isRemoveOk = await configSvc.RemoveConfig(dataId, group);
//        Console.WriteLine(isRemoveOk);
//        await Task.Delay(3000);

//        content = await configSvc.GetConfig(dataId, group, 5000);
//        Console.WriteLine(content);
//        await Task.Delay(300000);
//    }

//    internal class ConfigListener : IListener
//    {
//        public void ReceiveConfigInfo(string configInfo)
//        {
//            Console.WriteLine("recieve:" + configInfo);
//        }
//    }
//}


//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Serilog;
//using Serilog.Events;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        Log.Logger = new LoggerConfiguration()
//            .Enrich.FromLogContext()
//            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//            .MinimumLevel.Override("System", LogEventLevel.Warning)
//            .MinimumLevel.Debug()
//            .WriteTo.Console()
//            .CreateLogger();

//        try
//        {
//            Log.ForContext<Program>().Information("Application starting...");
//            CreateHostBuilder(args, Log.Logger).Build().Run();
//        }
//        catch (System.Exception ex)
//        {
//            Log.ForContext<Program>().Fatal(ex, "Application start-up failed!!");
//        }
//        finally
//        {
//            Log.CloseAndFlush();
//        }
//    }

//    public static IHostBuilder CreateHostBuilder(string[] args, Serilog.ILogger logger) =>
//        Host.CreateDefaultBuilder(args)
//            .ConfigureAppConfiguration((context, builder) =>
//            {
//                var c = builder.Build();
//                builder.AddNacosV2Configuration(c.GetSection("NacosConfig"), logAction: x => x.AddSerilog(logger));
//            })
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<Startup>().UseUrls("http://*:8787");
//            })
//            .UseSerilog();
//}
