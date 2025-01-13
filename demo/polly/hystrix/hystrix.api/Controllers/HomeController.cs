using System;
using System.Threading;
using System.Threading.Tasks;
using hystrix.core;
using hystrix.core.minimum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("~/swagger/index.html");
        }

        [HttpGet]
        [Route("test-ObsoleteFallbackServiceFilter")]
        [ObsoleteFallbackService]
        public void TestObsoleteFallbackServiceFilter()
        {

        }

        [HttpGet]
        [Route("test-FailAggregationFilter")]
        [FailAggregationFilter]
        public void TestFailAggregationFilter()
        {

        }

        [HttpGet]
        [Route("test-FallbackFilter")]
        [Fallback(FallbackMethod = nameof(TestFallbackFilterFallBack), RetryTimes = 3)]
        [TypeFilter(typeof(FallbackFilter))]
        public async Task<string> TestFallbackFilter()
        {
            Console.WriteLine("execute....");
            string s = null;
            s.ToString();

            await Task.CompletedTask;
            return "1";
        }

        private string TestFallbackFilterFallBack()
        {
            return "0";
        }
    }
}