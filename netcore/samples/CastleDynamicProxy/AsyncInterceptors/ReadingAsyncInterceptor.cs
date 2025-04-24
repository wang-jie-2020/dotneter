using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Demo.Aop
{
    public class ReadingAsyncInterceptor
    {
        public void InterceptSynchronous(IInvocation invocation)
        {
            Type returnType = invocation.Method.ReturnType;
            if (returnType == typeof(void))
            {
                Task task = InterceptAsync(invocation, ProceedSynchronous);
                if (!task.IsCompleted)
                {
                    Task.Run(() => task).GetAwaiter().GetResult();
                }

                return;
            }

            var method = this.GetType().GetMethod("HandleAsync", BindingFlags.Instance | BindingFlags.NonPublic)?.MakeGenericMethod(returnType);
            if (method == null)
            {
                throw new ArgumentException(nameof(method));
            }

            method.Invoke(this, new object[] { invocation });
        }

        private void HandleAsync<TResult>(IInvocation invocation)
        {
            Task<TResult> task = InterceptAsync(invocation, ProceedSynchronous<TResult>);
            if (!task.IsCompleted)
            {
                Task.Run(() => task).GetAwaiter().GetResult();
            }
        }

        private Task ProceedSynchronous(IInvocation invocation)
        {
            try
            {
                invocation.CaptureProceedInfo().Invoke();
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }

        private Task<TResult> ProceedSynchronous<TResult>(IInvocation invocation)
        {
            try
            {
                invocation.CaptureProceedInfo().Invoke();
                return Task.FromResult((TResult)invocation.ReturnValue);
            }
            catch (Exception e)
            {
                return Task.FromException<TResult>(e);
            }
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptAsync(invocation, ProceedAsynchronous);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InterceptAsync(invocation, ProceedAsynchronous<TResult>);
        }

        protected virtual async Task InterceptAsync(IInvocation invocation, Func<IInvocation, Task> proceed)
        {
            Console.WriteLine($"Starting Reading Method");

            await proceed(invocation).ConfigureAwait(false);

            Console.WriteLine($"Completed Reading Method");
        }

        protected virtual async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, Func<IInvocation, Task<TResult>> proceed)
        {
            Console.WriteLine($"Starting Reading Method");

            TResult result = await proceed(invocation).ConfigureAwait(false);

            Console.WriteLine($"Completed Reading Method");

            return result;
        }

        private async Task ProceedAsynchronous(IInvocation invocation)
        {
            invocation.CaptureProceedInfo().Invoke();

            var originalReturnValue = (Task)invocation.ReturnValue;
            await originalReturnValue.ConfigureAwait(false);
        }

        private async Task<TResult> ProceedAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.CaptureProceedInfo().Invoke();

            var originalReturnValue = (Task<TResult>)invocation.ReturnValue;
            TResult result = await originalReturnValue.ConfigureAwait(false);
            return result;
        }

    }
}
