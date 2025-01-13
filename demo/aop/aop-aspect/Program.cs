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
                //�滻����ΪAspectCore.Ioc���������߻���castleд��ioc����������ע��ķ�ʽĬ�ϰ�����Aspect-AOP������ע��
                //.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());

                //�滻����ΪAutofac��������AOP�����ע�ᣬ��Ҫ��CongureContainerʱ����
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}




