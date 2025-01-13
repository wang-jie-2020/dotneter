using Castle.DynamicProxy;

namespace Demo.Aop
{
    public class LoggingAsyncInterceptorAdapter : AsyncDeterminationInterceptor
    {
        private readonly LoggingAsyncInterceptor _loggingAsyncInterceptor;

        public LoggingAsyncInterceptorAdapter(LoggingAsyncInterceptor loggingAsyncInterceptor) : base(loggingAsyncInterceptor)
        {
            _loggingAsyncInterceptor = loggingAsyncInterceptor;
        }

        public override void Intercept(IInvocation invocation)
        {
            _loggingAsyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}
