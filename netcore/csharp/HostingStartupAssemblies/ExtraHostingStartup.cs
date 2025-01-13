using System;
using System.Collections.Generic;
using HostingStartupAssemblies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ExtraHostingStartup))]
namespace HostingStartupAssemblies
{
    public class ExtraHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            //添加Configuration
            /*
             * 
             * 处理配置有两种方法，具体取决于是希望托管启动的配置优先还是应用的配置优先：
             * 使用 ConfigureAppConfiguration 为应用提供配置，在应用的 ConfigureAppConfiguration 委托执行后加载配置。 使用此方法，托管启动配置优先于应用的配置。
             * 使用 UseConfiguration 为应用提供配置，在应用的 ConfigureAppConfiguration 委托执行之前加载配置。 使用此方法，应用的配置值优先于托管启动程序提供的值。
             */
            builder.ConfigureAppConfiguration((context, config) =>
            {
                Console.WriteLine("--配置--");
                var data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("extra-service-name", "extra-hosting-startup")
                };

                config.AddInMemoryCollection(data);
            });

            //添加ConfigureServices
            builder.ConfigureServices((context, services) =>
            {
                Console.WriteLine("--组件--");
                services.AddScoped(provider => new ExtraService { Name = "extra-service" });
            });

            //添加Configure---实际上附加管道处理也许不是一个好主意,会引起原管道的一些问题,但如果只在第一个或者最后一个,也许可行,绝不是这种语法
            //也许IStartupFilter是一个不错的点子
            //builder.Configure(app =>
            //{
            //    app.Use(async (context, next) =>
            //    {
            //        Console.WriteLine("self-pipeline");
            //        await next();
            //    });
            //});

            //IStartupFilter 可以将组件附加到pipeline中,但是,它的位置,即http请求的处理顺序永远在第一个(要么是最后)
            builder.ConfigureServices((context, services) =>
            {
                services.AddTransient<IStartupFilter, RequestSetOptionsStartupFilter>();
            });

        }
    }
}
