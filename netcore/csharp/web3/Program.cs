using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web3
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
                    //1.����
                    webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "HostingStartupAssemblies");
                    //2.�������� ��launchSettings.json
                    //3.������� ����֧��???

                    webBuilder.UseStartup<Startup>();

                    //ͨ��UseSetting�ķ�ʽָ�����򼯵�����
                    //webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "HostStartupLib");
                    //webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "HostStartupLib;HostStartupLib2");
                });
    }
}
