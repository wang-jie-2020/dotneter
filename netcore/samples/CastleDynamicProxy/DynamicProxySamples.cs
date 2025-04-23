using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;

namespace CastleDynamicProxy;

/*
    切面拦截实际上也是在动态代理之上的一层,不过它的过程中包含了对被代理对象的访问
    被代理对象完全动态生成也是可能的,也就是说其中包含的业务完全由拦截器实现,最经典的案例是动态代理HttpClient的访问,比如ABP中的DynamicWebApi、Refit、WebApiCore
 */
public class DynamicProxySamples
{
    // 一个简单的代理示例
    void Method1()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<Cat>().As<ICat>();
        builder.RegisterType<CatProxyInterceptor>();

        builder.RegisterType<CatProxy>()
            .InterceptedBy(typeof(CatProxyInterceptor))
            .EnableClassInterceptors(ProxyGenerationOptions.Default, typeof(ICat));

        var container = builder.Build();

        var cat = (ICat)container.Resolve<CatProxy>();
        Console.WriteLine(cat.Hello());
    }

    // 很多场景下都是直接代理接口,在接口里以attribute标注feature
    void Method2()
    {
        var interceptorType = typeof(DynamicProxyInterceptor<>).MakeGenericType(typeof(ISomething));
        var interceptorInstance = Activator.CreateInstance(interceptorType);

        var proxyGenerator = new ProxyGenerator();
        ISomething something = proxyGenerator.CreateInterfaceProxyWithoutTarget<ISomething>((IInterceptor)interceptorInstance);
        Console.WriteLine(something.Hello());
    }
}