using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hystrix.core
{
    public class FailAggregationFilter : Attribute, IAsyncActionFilter
    {
        /// <summary>
        /// 最多重试几次，如果为0则不重试
        /// </summary>
        public int RetryTimes { get; set; } = 0;

        /// <summary>
        /// 重试间隔的毫秒数
        /// </summary>
        public int RetryInterval { get; set; } = 100;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine($"拦截{nameof(FailAggregationFilter)}");
            await next();
        }
    }
}
