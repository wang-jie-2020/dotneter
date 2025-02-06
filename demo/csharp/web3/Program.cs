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
                    //1.编码
                    webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "HostingStartupAssemblies");
                    //2.环境变量 见launchSettings.json
                    //3.命令参数 好像不支持???

                    webBuilder.UseStartup<Startup>();

                    //通过UseSetting的方式指定程序集的名称
                    //webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "HostStartupLib");
                    //webBuilder.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "HostStartupLib;HostStartupLib2");
                });
    }
}
