using AspNetCore.Consul.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.WebApi.Client
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

                        //http�˿�
                        options.ListenAnyIP(serviceInfo.Port);
                    });

                }).UseConsul(options =>//ʹ��Consul��kv������
                {
                    options.Address = "http://192.168.209.128:18401";
                    options.Datacenter = "dc1";
                    //options.Token = "token";
                    options.Prefix = "Root/Consul";

                    //ʹ������ʽ��ѯʵ���ȸ���
                    options.Mode = WatchMode.Poll;
                    options.Interval = TimeSpan.FromMinutes(3);
                });
    }
}
