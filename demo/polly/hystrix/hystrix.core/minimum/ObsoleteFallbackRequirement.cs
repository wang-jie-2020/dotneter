using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hystrix.core.minimum
{
    public class ObsoleteFallbackRequirement
    {
        public int RetryTimes { get; set; } = 0;
    }
}
