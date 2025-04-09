using Volo.Abp.DynamicProxy;

namespace AbpWeb.Interceptors;

public class SimpleInterceptor: AbpInterceptor
{
    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        await Task.Delay(5);
        await invocation.ProceedAsync();
        await Task.Delay(5);
    }
}