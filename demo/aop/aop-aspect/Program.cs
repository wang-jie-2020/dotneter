using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using AspectCore.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Demo.Aop;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                //替换容器为AspectCore.Ioc，这是作者基于castle写的ioc容器，这种注册的方式默认包含了Aspect-AOP相关组件注册
                //.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());

                //替换容器为Autofac，不包括AOP的组件注册，需要在CongureContainer时增加
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}




