using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;

namespace CastleDynamicProxy;

/*
    Autofac.Extras.DynamicProxy ProxyGenerator

    EnableClassInterceptors :
        registration.ActivatorData.ImplementationType =
            ProxyGenerator.ProxyBuilder.CreateClassProxyType(
                registration.ActivatorData.ImplementationType,
                additionalInterfaces ?? Type.EmptyTypes,
                options);

    EnableInterfaceInterceptors :
            ctx.Instance = options == null
                ? ProxyGenerator.CreateInterfaceProxyWithTarget(theInterface, interfaces, ctx.Instance, interceptors)
                : ProxyGenerator.CreateInterfaceProxyWithTarget(theInterface, interfaces, ctx.Instance, options, interceptors);
 */
public class InterceptorSamples
{
    // class proxy
    void Method1()
    {
        CIntegration();
        CGeneratorWithTarget();
        CGeneratorWithoutTarget();
    }

    void CIntegration()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<CatInterceptor>();
        builder.RegisterType<Cat>().AsSelf().InterceptedBy(typeof(CatInterceptor)).EnableClassInterceptors();
        
        var container = builder.Build();
        var cat = container.Resolve<Cat>();

        Console.WriteLine("1----------");
        cat.Eat();  // 猫在吃东西
        Console.WriteLine("2----------");
        cat.Drink();  // CatInterceptor-Drink-Begin    猫在喝水    CatInterceptor-Drink-End
        Console.WriteLine("3----------");
        cat.RaiseCat(); // 猫在吃东西    CatInterceptor-Drink-Begin    猫在喝水    CatInterceptor-Drink-End
        Console.WriteLine("4----------");
        cat.FeedCat(); // CatInterceptor-FeedCat-Begin    猫在吃东西    CatInterceptor-Drink-Begin    猫在喝水    CatInterceptor-Drink-End    CatInterceptor-FeedCat-End
    }

    void CGeneratorWithTarget()
    {
        var proxyGenerator = new ProxyGenerator();
        var cat = proxyGenerator.CreateClassProxyWithTarget(new Cat(), ProxyGenerationOptions.Default, new CatInterceptor());
        
        Console.WriteLine("1----------");
        cat.Eat();  // 猫在吃东西
        Console.WriteLine("2----------");
        cat.Drink();  // CatInterceptor-Drink-Begin    猫在喝水    CatInterceptor-Drink-End
        Console.WriteLine("3----------");
        cat.RaiseCat(); // 猫在吃东西    CatInterceptor-Drink-Begin    猫在喝水    CatInterceptor-Drink-End
        Console.WriteLine("4----------");
        cat.FeedCat(); // CatInterceptor-FeedCat-Begin    猫在吃东西     猫在喝水    CatInterceptor-FeedCat-End
    }
    
    void CGeneratorWithoutTarget()
    {
        var proxyGenerator = new ProxyGenerator();
        var cat = proxyGenerator.CreateClassProxy<Cat>(ProxyGenerationOptions.Default, new CatInterceptor());
        
        Console.WriteLine("1----------");
        cat.Eat();  // 猫在吃东西
        Console.WriteLine("2----------");
        cat.Drink();  // CatInterceptor-Drink-Begin    猫在喝水    CatInterceptor-Drink-End
        Console.WriteLine("3----------");
        cat.RaiseCat(); // 猫在吃东西    CatInterceptor-Drink-Begin    猫在喝水    CatInterceptor-Drink-End
        Console.WriteLine("4----------");
        cat.FeedCat(); // CatInterceptor-FeedCat-Begin    猫在吃东西    CatInterceptor-Drink-Begin     猫在喝水    CatInterceptor-Drink-End    CatInterceptor-FeedCat-End
    }

    // interface proxy
    void Method2()
    {
        //IIntegration();
        //IGeneratorWithTarget();
        //IGeneratorWithoutTarget();
        IGeneratorWithoutTarget2();
    }
    
     void IIntegration()
    {
        var builder = new ContainerBuilder();
        builder.RegisterType<CatInterceptor>();
        builder.RegisterType<Cat>().As<ICat>().InterceptedBy(typeof(CatInterceptor)).EnableInterfaceInterceptors();
        
        var container = builder.Build();
        var cat = container.Resolve<ICat>();

        Console.WriteLine("1----------");
        cat.Eat();  // CatInterceptor-Eat-Begin    猫在吃东西    CatInterceptor-Eat-End
        Console.WriteLine("2----------");
        cat.Drink();  // CatInterceptor-Drink-Begin    猫在喝水    CatInterceptor-Drink-End
        Console.WriteLine("3----------");
        cat.RaiseCat(); // CatInterceptor-RaiseCat-Begin    猫在吃东西    猫在喝水    CatInterceptor-RaiseCat-End
        Console.WriteLine("4----------");
        cat.FeedCat(); // CatInterceptor-FeedCat-Begin    猫在吃东西    猫在喝水    CatInterceptor-FeedCat-End
    }

    void IGeneratorWithTarget()
    {
        var proxyGenerator = new ProxyGenerator();
        var cat = proxyGenerator.CreateInterfaceProxyWithTarget<ICat>(new Cat(), ProxyGenerationOptions.Default, new CatInterceptor());
        
        Console.WriteLine("1----------");
        cat.Eat();  // CatInterceptor-Eat-Begin    猫在吃东西    CatInterceptor-Eat-End
        Console.WriteLine("2----------");
        cat.Drink();  // CatInterceptor-Drink-Begin    猫在喝水    CatInterceptor-Drink-End
        Console.WriteLine("3----------");
        cat.RaiseCat(); // CatInterceptor-RaiseCat-Begin    猫在吃东西    猫在喝水    CatInterceptor-RaiseCat-End
        Console.WriteLine("4----------");
        cat.FeedCat(); // CatInterceptor-FeedCat-Begin    猫在吃东西    猫在喝水    CatInterceptor-FeedCat-End
    }
    
    void IGeneratorWithoutTarget()
    {
        var proxyGenerator = new ProxyGenerator();
        var cat = proxyGenerator.CreateInterfaceProxyWithoutTarget<ICat>(ProxyGenerationOptions.Default, new CatInterceptor());
        
        // 报错  The interceptor attempted to 'Proceed' for method 'void Eat()' which has no target
        // invocation.Proceed() 需要实例的方法,如果可以忽略它那么就ok,见下面
        cat.Eat();  
    }
    
    void IGeneratorWithoutTarget2()
    {
        var proxyGenerator = new ProxyGenerator();
        var cat = proxyGenerator.CreateInterfaceProxyWithoutTarget<ICat>(ProxyGenerationOptions.Default, new EmptyInterceptor());
        
        cat.Eat();  
    }
}