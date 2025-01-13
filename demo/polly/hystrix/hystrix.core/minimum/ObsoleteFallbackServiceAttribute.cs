using System;
using Microsoft.AspNetCore.Mvc;

namespace hystrix.core.minimum
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ObsoleteFallbackServiceAttribute : TypeFilterAttribute
    {
        public ObsoleteFallbackServiceAttribute() : base(typeof(ObsoleteFallbackFilter))
        {
            Arguments = new object[]
            {
                new ObsoleteFallbackRequirement()
                {
                    RetryTimes = 99
                }
            };
        }
    }
}
