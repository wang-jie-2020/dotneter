using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Demo.Aop
{
    public class LoggingAsyncInterceptor : AsyncInterceptorBase
    {
        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            Console.WriteLine($"{invocation.Method.Name}:Starting Void Method");

            await proceed(invocation, proceedInfo).ConfigureAwait(false);

            Console.WriteLine($"{invocation.Method.Name}:Completed Void  Method");
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            Console.WriteLine($"{invocation.Method.Name}:Starting Result Method");

            TResult result = await proceed(invocation, proceedInfo).ConfigureAwait(false);

            Console.WriteLine($"{invocation.Method.Name}:Completed Result Method");

            return result;
        }
    }
}
