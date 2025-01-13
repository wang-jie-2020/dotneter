using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Aop
{
    /*
     *  通常都会存在一些组件注入,此处就会出现标注不太靠谱的情况
     *      当然可以通过Ioc提供的属性注入特性处理,但是还是非常不推荐这么做的,又不是java
     */
    public class CustomerInterceptorAttribute : AspectCore.DynamicProxy.AbstractInterceptorAttribute
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CustomerInterceptorAttribute(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Console.WriteLine(" CustomerInterceptor begin");
                await next(context);
            }
            catch
            {
                Console.WriteLine(" CustomerInterceptor error");
                throw;
            }
            finally
            {
                Console.WriteLine(" CustomerInterceptor end");
            }
        }
    }
}
