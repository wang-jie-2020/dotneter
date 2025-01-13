using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Aop
{
    public class ManagerInterceptorAttribute : AspectCore.DynamicProxy.AbstractInterceptorAttribute
    {
        private readonly string _name;

        public ManagerInterceptorAttribute()
        {

        }

        public ManagerInterceptorAttribute(string name)
        {
            _name = name;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                Console.WriteLine($"  {nameof(ManagerInterceptorAttribute)} begin" + (_name == null ? "" : ":" + _name));
                await next(context);
            }
            catch
            {
                Console.WriteLine($"  {nameof(ManagerInterceptorAttribute)} error" + (_name == null ? "" : ":" + _name));
                throw;
            }
            finally
            {
                Console.WriteLine($"  {nameof(ManagerInterceptorAttribute)} end" + (_name == null ? "" : ":" + _name));
            }
        }
    }
}
