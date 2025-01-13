using System;
using Castle.DynamicProxy;

namespace Demo.Aop
{
    public class ManagerInterceptor : IInterceptor
    {
        private readonly string _name;

        public ManagerInterceptor()
        {

        }

        public ManagerInterceptor(string name)
        {
            _name = name;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                Console.WriteLine($"  {nameof(ManagerInterceptor)} begin" + (_name == null ? "" : ":" + _name));
                invocation.Proceed();
            }
            catch
            {
                Console.WriteLine($"  {nameof(ManagerInterceptor)} error" + (_name == null ? "" : ":" + _name));
                throw;
            }
            finally
            {
                Console.WriteLine($"  {nameof(ManagerInterceptor)} end" + (_name == null ? "" : ":" + _name));
            }
        }
    }
}