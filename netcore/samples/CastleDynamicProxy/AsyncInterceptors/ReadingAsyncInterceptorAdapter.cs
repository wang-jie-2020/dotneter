using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Castle.DynamicProxy;

namespace Demo.Aop
{
    public class ReadingAsyncInterceptorAdapter : IInterceptor
    {
        public ReadingAsyncInterceptor ReadingAsyncInterceptor { get; }

        public ReadingAsyncInterceptorAdapter(ReadingAsyncInterceptor readingAsyncInterceptor)
        {
            ReadingAsyncInterceptor = readingAsyncInterceptor;
        }


        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        /*
            这块的思路描述
                invocation.Proceed() 会让拦截对象执行具体代码,那么就把invocation对象拿出来,在需要执行的时候再调用而已
                当然这里会有一个Task<T>的包装问题,通过构建泛型方法就可以了
        /*	
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        public void Intercept(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;

            if (returnType == typeof(Task))
            {
                ReadingAsyncInterceptor.InterceptAsynchronous(invocation);
            }
            else if (returnType.GetTypeInfo().IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var method = this.GetType().GetMethod("HandleAsync", BindingFlags.Instance | BindingFlags.NonPublic)?.MakeGenericMethod(returnType.GenericTypeArguments[0]);
                if (method == null)
                {
                    throw new ArgumentException(nameof(method));
                }

                //改一下写法,这句的意思和委托写法一致
                method.Invoke(this, new object[] { invocation });

                //var delegateMethod = (GenericAsyncHandler)method.CreateDelegate(typeof(GenericAsyncHandler));
                //delegateMethod.Invoke(invocation);
            }
            else
            {
                ReadingAsyncInterceptor.InterceptSynchronous(invocation);
            }
        }

        private delegate void GenericAsyncHandler(IInvocation invocation);

        private void HandleAsync<TResult>(IInvocation invocation)
        {
            ReadingAsyncInterceptor.InterceptAsynchronous<TResult>(invocation);
        }
    }
}