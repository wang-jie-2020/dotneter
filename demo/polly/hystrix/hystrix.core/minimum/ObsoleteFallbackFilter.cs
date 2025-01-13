using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hystrix.core.minimum
{
    public class ObsoleteFallbackFilter : IAsyncActionFilter
    {
        private readonly ObsoleteFallbackRequirement _attribute;

        public ObsoleteFallbackFilter(ObsoleteFallbackRequirement attribute)
        {
            _attribute = attribute;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine($"拦截{nameof(ObsoleteFallbackFilter)}");
            await next();
        }
    }
}
