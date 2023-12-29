using AspNetCore.Consul.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AspNetCore.WebApi.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        var serviceInfo = options.ApplicationServices.GetRequiredService<IOptions<ServiceInfo>>().Value;

                        //http端口
                        options.ListenAnyIP(serviceInfo.Port);

                        //grpc端口
                        options.ListenAnyIP(serviceInfo.GrpcPort, listenOptions =>
                        {
                            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                        });
                    });

                    //使用Consul的kv做配置
                    webBuilder.ConfigureAppConfiguration(builder =>
                    {
                        builder.AddConsul(options =>
                        {
                            options.Address = "http://192.168.209.128:18401";
                            options.Datacenter = "dc1";
                            //options.Token = "token";
                            options.Prefix = "Root/Consul";

                            //使用consul-template或者watch来实现热更新，需要在Startup的Configure中使用UseConsulWatch拦截更新配置请求
                            options.Mode = WatchMode.Watch;
                            options.ReloadName = "demo";
                        });
                    });
                });
    }
}
