using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("1 or 2");
            var ver = Console.ReadLine();
            if (ver == "1")
            {
                Startup.Ver = "1";
            }
            else if (ver == "2")
            {
                Startup.Ver = "2";
            }
            else
            {
                throw new Exception();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}