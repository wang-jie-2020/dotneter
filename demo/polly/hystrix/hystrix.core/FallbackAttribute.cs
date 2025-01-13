using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace hystrix.core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FallbackAttribute : Attribute
    {
        /// <summary>
        /// 最多重试几次，如果为0则不重试
        /// </summary>
        public int RetryTimes { get; set; } = 0;

        /// <summary>
        /// 重试间隔的毫秒数
        /// </summary>
        public int RetryInterval { get; set; } = 100;

        /// <summary>
        /// 是否启用熔断
        /// </summary>
        public bool EnableCircuitBreaker { get; set; } = false;

        /// <summary>
        /// 熔断前出现允许错误几次
        /// </summary>
        public int CircuitBreakerAllowedExceptionCount { get; set; } = 3;

        /// <summary>
        /// 熔断多长时间（毫秒）
        /// </summary>
        public int CircuitBreakerInterval { get; set; } = 1000;

        /// <summary>
        /// 回落事件
        /// </summary>
        public string FallbackMethod { get; set; } = string.Empty;

    }
}
