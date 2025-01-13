using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Demo.Aop
{
    public class ReadingAsyncInterceptor : AsyncInterceptorBase
    {
        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            Console.WriteLine($"Starting Reading Method");

            await proceed(invocation, proceedInfo).ConfigureAwait(false);

            Console.WriteLine($"Completed Reading Method");
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            Console.WriteLine($"Starting Reading Method");

            TResult result = await proceed(invocation, proceedInfo).ConfigureAwait(false);

            Console.WriteLine($"Completed Reading Method");

            return result;
        }
    }
}
