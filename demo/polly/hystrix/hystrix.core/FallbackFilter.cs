using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Polly;

namespace hystrix.core
{
    public class FallbackFilter : IAsyncActionFilter, IAsyncExceptionFilter
    {
        private static ConcurrentDictionary<MethodInfo, IAsyncPolicy> _policies = new ConcurrentDictionary<MethodInfo, IAsyncPolicy>();

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            return;


            if (!(context.ActionDescriptor is ControllerActionDescriptor actionDescriptor))
            {
                await next();
                return;
            }

            var methodInfo = actionDescriptor.MethodInfo;
            var fallbackAttr = FallbackHelper.GetFallbackAttributeOrNull(methodInfo);

            if (fallbackAttr == null)
            {
                await next();
                return;
            }

            _policies.TryGetValue(methodInfo, out IAsyncPolicy policy);
            if (policy == null)
            {
                lock (_policies)
                {
                    if (policy == null)
                    {
                        policy = Policy.NoOpAsync();

                        if (fallbackAttr.EnableCircuitBreaker)
                        {
                            policy = policy.WrapAsync(Policy.Handle<Exception>().CircuitBreakerAsync(fallbackAttr.CircuitBreakerAllowedExceptionCount, TimeSpan.FromMilliseconds(fallbackAttr.CircuitBreakerInterval)));
                        }

                        if (fallbackAttr.RetryTimes > 0)
                        {
                            policy = policy.WrapAsync(Policy.Handle<Exception>().WaitAndRetryAsync(fallbackAttr.RetryTimes, i => TimeSpan.FromMilliseconds(fallbackAttr.RetryInterval),
                                onRetryAsync: (async (exception, timespan, index, ctx) =>
                                {
                                    Console.WriteLine($"第{index}次重试");
                                    await Task.CompletedTask;
                                })));
                        }

                        //var policyFallBack = Policy.Handle<Exception>().FallbackAsync(
                        //async (ctx, t) =>
                        //{
                        //    var internalContext = (ActionExecutingContext)ctx["actionExecutingContext"];
                        //    var internalActionDescriptor = (ControllerActionDescriptor)internalContext.ActionDescriptor;

                        //    var fallBackMethod = actionDescriptor.MethodInfo.DeclaringType.GetMethod(fallbackAttr.FallbackMethod);
                        //    var fallBackResult = fallBackMethod.Invoke(internalContext.Controller, internalContext.ActionArguments.Values.ToArray());

                        //    internalContext.Result = (IActionResult)fallBackResult;
                        //    await Task.CompletedTask;
                        //},
                        //async (ex, t) =>
                        //{
                        //    await Task.CompletedTask;
                        //});

                        //policy = policyFallBack.WrapAsync(policy);
                    }

                    _policies.TryAdd(methodInfo, policy);
                }
            }

            await policy.ExecuteAsync((ctx) => next(), new Context
            {
                ["actionExecutingContext"] = context
            });

            //await next();
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;
            //context.ExceptionHandled = true;
        }
    }
}
