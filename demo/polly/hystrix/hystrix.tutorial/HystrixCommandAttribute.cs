using AspectCore.DynamicProxy;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace hystrix
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HystrixCommandAttribute : AbstractInterceptorAttribute
    {
        public string FallBackMethod { get; set; }

        /// <summary>
        /// 最多重试几次，如果为0则不重试
        /// </summary>
        public int MaxRetryTimes { get; set; } = 0;

        /// <summary>
        /// 重试间隔的毫秒数
        /// </summary>
        public int RetryIntervalMilliseconds { get; set; } = 100;

        /// <summary>
        /// 是否启用熔断
        /// </summary>
        public bool EnableCircuitBreaker { get; set; } = false;

        /// <summary>
        /// 熔断前出现允许错误几次
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 3;

        /// <summary>
        /// 熔断多长时间（毫秒）
        /// </summary>
        public int MillisecondsOfBreak { get; set; } = 1000;

        /// <summary>
        /// 执行超过多少毫秒则认为超时（0表示不检测超时）
        /// </summary>
        public int TimeOutMilliseconds { get; set; } = 0;

        private static ConcurrentDictionary<MethodInfo, IAsyncPolicy> _policies = new ConcurrentDictionary<MethodInfo, IAsyncPolicy>();

        public HystrixCommandAttribute(string fallBackMethod)
        {
            this.FallBackMethod = fallBackMethod;
        }

        /*
         *
            理解作者描述的两个问题:
                1.CircuitBreaker需要保持Policy一致,那么Policy需要保持;若需要保持,那么键不能是每次代理输出的对象
                2.Policy的Context会保持,在这种场景下需要额外进行赋值
                3.闭包问题,其实是由于Invoke-context在Fallback中被引入到了Policy中,而Policy是会保持的
         *
         */


        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            _policies.TryGetValue(context.ServiceMethod, out IAsyncPolicy policy);
            if (policy == null)
            {
                lock (_policies)
                {
                    if (policy == null)
                    {
                        policy = Policy.NoOpAsync();

                        if (EnableCircuitBreaker)
                        {
                            policy = policy.WrapAsync(Policy.Handle<Exception>().CircuitBreakerAsync(ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(MillisecondsOfBreak)));
                        }

                        if (TimeOutMilliseconds > 0)
                        {
                            policy = policy.WrapAsync(Policy.TimeoutAsync(() => TimeSpan.FromMilliseconds(TimeOutMilliseconds), Polly.Timeout.TimeoutStrategy.Pessimistic));
                        }

                        if (MaxRetryTimes > 0)
                        {
                            policy = policy.WrapAsync(Policy.Handle<Exception>().WaitAndRetryAsync(MaxRetryTimes, i => TimeSpan.FromMilliseconds(RetryIntervalMilliseconds),
                                 (async (exception, span, arg3, arg4) =>
                                {
                                    Console.WriteLine($"retrying---{arg3}");
                                    await Task.CompletedTask;
                                })));
                        }

                        var policyFallBack = Policy.Handle<Exception>().FallbackAsync(
                async (ctx, t) =>
                            {
                                AspectContext aspectContext = (AspectContext)ctx["aspectContext"];

                                Debug.Assert(aspectContext.ImplementationMethod.DeclaringType != null, "aspectContext.ImplementationMethod.DeclaringType != null");
                                var fallBackMethod = aspectContext.ImplementationMethod.DeclaringType.GetMethod(this.FallBackMethod);

                                Debug.Assert(fallBackMethod != null, nameof(fallBackMethod) + " != null");
                                var fallBackResult = fallBackMethod.Invoke(aspectContext.Implementation, aspectContext.Parameters);

                                aspectContext.ReturnValue = fallBackResult;
                                await Task.CompletedTask;
                            },
                async (ex, t) =>
                            {
                                await Task.CompletedTask;
                            });

                        policy = policyFallBack.WrapAsync(policy);
                    }

                    _policies.TryAdd(context.ServiceMethod, policy);
                }
            }

            await policy.ExecuteAsync((ctx) => next(context), new Context
            {
                ["aspectContext"] = context //这里解决闭包问题
            });
        }

        ////仓库源码
        //public override async Task Invoke(AspectContext context, AspectDelegate next)
        //{
        //    //一个HystrixCommand中保持一个policy对象即可
        //    //其实主要是CircuitBreaker要求对于同一段代码要共享一个policy对象
        //    //根据反射原理，同一个方法的MethodInfo是同一个对象，但是对象上取出来的HystrixCommandAttribute
        //    //每次获取的都是不同的对象，因此以MethodInfo为Key保存到policies中，确保一个方法对应一个policy实例
        //    policies.TryGetValue(context.ServiceMethod, out Policy policy);
        //    lock (policies)//因为Invoke可能是并发调用，因此要确保policies赋值的线程安全
        //    {
        //        if (policy == null)
        //        {
        //            policy = Policy.NoOpAsync();//创建一个空的Policy
        //            if (EnableCircuitBreaker)
        //            {
        //                policy = policy.WrapAsync(Policy.Handle<Exception>().CircuitBreakerAsync(ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(MillisecondsOfBreak)));
        //            }
        //            if (TimeOutMilliseconds > 0)
        //            {
        //                policy = policy.WrapAsync(Policy.TimeoutAsync(() => TimeSpan.FromMilliseconds(TimeOutMilliseconds), Polly.Timeout.TimeoutStrategy.Pessimistic));
        //            }
        //            if (MaxRetryTimes > 0)
        //            {
        //                policy = policy.WrapAsync(Policy.Handle<Exception>().WaitAndRetryAsync(MaxRetryTimes, i => TimeSpan.FromMilliseconds(RetryIntervalMilliseconds)));
        //            }
        //            Policy policyFallBack = Policy
        //            .Handle<Exception>()
        //            .FallbackAsync(async (ctx, t) =>
        //            {
        //                AspectContext aspectContext = (AspectContext)ctx["aspectContext"];
        //                //var fallBackMethod = context.ServiceMethod.DeclaringType.GetMethod(this.FallBackMethod);
        //                //merge this issue: https://github.com/yangzhongke/RuPeng.HystrixCore/issues/2
        //                var fallBackMethod = context.ImplementationMethod.DeclaringType.GetMethod(this.FallBackMethod);
        //                Object fallBackResult = fallBackMethod.Invoke(context.Implementation, context.Parameters);
        //                //不能如下这样，因为这是闭包相关，如果这样写第二次调用Invoke的时候context指向的
        //                //还是第一次的对象，所以要通过Polly的上下文来传递AspectContext
        //                //context.ReturnValue = fallBackResult;
        //                aspectContext.ReturnValue = fallBackResult;
        //            }, async (ex, t) => { });

        //            policy = policyFallBack.WrapAsync(policy);
        //            //放入
        //            policies.TryAdd(context.ServiceMethod, policy);
        //        }
        //    }

        //    //把本地调用的AspectContext传递给Polly，主要给FallbackAsync中使用，避免闭包的坑
        //    Context pollyCtx = new Context();
        //    pollyCtx["aspectContext"] = context;

        //    //Install-Package Microsoft.Extensions.Caching.Memory
        //    if (CacheTTLMilliseconds > 0)
        //    {
        //        //用类名+方法名+参数的下划线连接起来作为缓存key
        //        string cacheKey = "HystrixMethodCacheManager_Key_" + context.ServiceMethod.DeclaringType
        //                                                           + "." + context.ServiceMethod + string.Join("_", context.Parameters);
        //        //尝试去缓存中获取。如果找到了，则直接用缓存中的值做返回值
        //        if (memoryCache.TryGetValue(cacheKey, out var cacheValue))
        //        {
        //            context.ReturnValue = cacheValue;
        //        }
        //        else
        //        {
        //            //如果缓存中没有，则执行实际被拦截的方法
        //            await policy.ExecuteAsync(ctx => next(context), pollyCtx);
        //            //存入缓存中
        //            using (var cacheEntry = memoryCache.CreateEntry(cacheKey))
        //            {
        //                cacheEntry.Value = context.ReturnValue;
        //                cacheEntry.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMilliseconds(CacheTTLMilliseconds);
        //            }
        //        }
        //    }
        //    else//如果没有启用缓存，就直接执行业务方法
        //    {
        //        await policy.ExecuteAsync(ctx => next(context), pollyCtx);
        //    }
        //}
    }
}
