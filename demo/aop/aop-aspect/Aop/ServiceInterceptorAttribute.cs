//using System;
//using System.Threading.Tasks;
//using AspectCore.DynamicProxy;

//namespace Demo.Aop
//{
//    public class ServiceInterceptorAttribute : AspectCore.DynamicProxy.AbstractInterceptorAttribute
//    {
//        private readonly string _name;

//        public ServiceInterceptorAttribute()
//        {

//        }

//        public ServiceInterceptorAttribute(string name)
//        {
//            _name = name;
//        }

//        public override async Task Invoke(AspectContext context, AspectDelegate next)
//        {
//            try
//            {
//                Console.WriteLine($"  {nameof(ServiceInterceptorAttribute)} begin" + (_name == null ? "" : ":" + _name));
//                await next(context);
//            }
//            catch
//            {
//                Console.WriteLine($"  {nameof(ServiceInterceptorAttribute)} error" + (_name == null ? "" : ":" + _name));
//                throw;
//            }
//            finally
//            {
//                Console.WriteLine($"  {nameof(ServiceInterceptorAttribute)} end" + (_name == null ? "" : ":" + _name));
//            }
//        }
//    }
//}
