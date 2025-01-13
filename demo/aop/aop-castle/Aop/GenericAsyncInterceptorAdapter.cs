using Castle.DynamicProxy;

namespace Demo.Aop
{
    public class GenericAsyncInterceptorAdapter<TAsyncInterceptor> : AsyncDeterminationInterceptor
        where TAsyncInterceptor : IAsyncInterceptor
    {
        public GenericAsyncInterceptorAdapter(TAsyncInterceptor asyncInterceptor) : base(asyncInterceptor)
        {

        }
    }
}
