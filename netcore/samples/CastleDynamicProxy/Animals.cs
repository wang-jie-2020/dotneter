using Castle.DynamicProxy;

namespace CastleDynamicProxy;

public interface ICat
{
    void Eat();

    void Drink();

    void RaiseCat();

    void FeedCat();

    string Hello();
}

public class Cat : ICat
{
    public void Eat()
    {
        Console.WriteLine("猫在吃东西");
    }

    public virtual void Drink()
    {
        Console.WriteLine("猫在喝水");
    }

    public void RaiseCat()
    {
        Eat();
        Drink();
    }

    public virtual void FeedCat()
    {
        Eat();
        Drink();
    }

    public string Hello()
    {
        return "Hello,I am a Cat!";
    }
}

public class CatInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var method = invocation.Method;

        Console.WriteLine($"CatInterceptor-{method.Name}-Begin");
        invocation.Proceed();
        Console.WriteLine($"CatInterceptor-{method.Name}-End");
    }
}

public class EmptyInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var method = invocation.Method;
        var info = invocation.CaptureProceedInfo();

        Console.WriteLine($"EmptyInterceptor-{method.Name}");
    }
}

public class CatProxy
{
}

public class CatProxyInterceptor : IInterceptor
{
    private readonly ICat _cat;

    public CatProxyInterceptor(ICat cat)
    {
        _cat = cat;
    }

    public void Intercept(IInvocation invocation)
    {
        invocation.ReturnValue = _cat.Hello();
    }
}

public interface ISomething
{
    string Hello();
}

public class DynamicProxyInterceptor<T> : IInterceptor
{
    public DynamicProxyInterceptor()
    {

    }

    public void Intercept(IInvocation invocation)
    {
        var method = invocation.Method;
        
        Console.WriteLine($"正在代理---{typeof(T).FullName}---{method.ReflectedType.FullName}--{method.Name}");
        
        invocation.ReturnValue = "hello, world!";
    }
}