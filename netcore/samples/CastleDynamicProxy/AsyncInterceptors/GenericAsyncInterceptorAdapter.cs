using Castle.DynamicProxy;

namespace CastleDynamicProxy.AsyncInterceptors
{
    public class GenericAsyncInterceptorAdapter<TAsyncInterceptor> : AsyncDeterminationInterceptor
        where TAsyncInterceptor : IAsyncInterceptor
    {
        public GenericAsyncInterceptorAdapter(TAsyncInterceptor asyncInterceptor) : base(asyncInterceptor)
        {

        }
    }
}
