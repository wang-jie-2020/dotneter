using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Castle.DynamicProxy;

namespace Demo.Aop
{
    /// <summary>
    /// 实现异步拦截器的简单原理演示
    /// </summary>
    public class NoPackingAsyncInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //Do before...

            invocation.Proceed();

            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(Task))
            {
                Func<Task> continuation = async () =>
                {
                    await (Task)invocation.ReturnValue;

                    //Do after...
                };
                invocation.ReturnValue = continuation();
            }
            else if (returnType.GetTypeInfo().IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                invocation.ReturnValue = this.GetType().GetMethod("HandleAsync")?
                    .MakeGenericMethod(returnType.GenericTypeArguments[0])
                    .Invoke(this, new object[] { invocation.ReturnValue });
            }
            else
            {
                //Do after...
            }
        }

        public async Task<T> HandleAsync<T>(Task<T> task)
        {
            var result = await task;

            //Do after...
            return result;
        }
    }
}