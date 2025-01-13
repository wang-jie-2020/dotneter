using System;
using Castle.DynamicProxy;

namespace Demo.Aop
{
    public class ServiceInterceptor : IInterceptor
    {
        private readonly string _name;

        public ServiceInterceptor()
        {

        }

        public ServiceInterceptor(string name)
        {
            _name = name;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                Console.WriteLine($"  {nameof(ServiceInterceptor)} begin" + (_name == null ? "" : ":" + _name));
                invocation.Proceed();
            }
            catch
            {
                Console.WriteLine($"  {nameof(ServiceInterceptor)} error" + (_name == null ? "" : ":" + _name));
                throw;
            }
            finally
            {
                Console.WriteLine($"  {nameof(ServiceInterceptor)} end" + (_name == null ? "" : ":" + _name));
            }
        }
    }
}
