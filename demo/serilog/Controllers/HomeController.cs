using System;
using System.Diagnostics;
using Demo.Filter;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("日志 Home!");
            _logger.LogError("出错了!!!{0}", DateTime.Now);

            // var log = Log.ForContext<CustomFilter>();
            // var user = new User { Id = 1, Name = "Adam", Created = DateTime.Now };
            // log.Information("Created {@User} on {Created}", user, DateTime.Now);

            return View();
        }

        public IActionResult Privacy()
        {
            var order = new Order { OrderNo = "1234567" };
            var operation = new Operation { OperationName = "Start|Ongoing|End" };

            var log = Log.ForContext<CustomFilter>()
                .ForContext("Order", order)
                .ForContext("OrderNo", order.OrderNo, true)
                .ForContext("OrderOperation", operation, true)
                .ForContext(typeof(Operation).FullName, operation, true);

            log.Information("ongoing");

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }

    public class Order
    {
        public string OrderNo { get; set; }
    }

    public class Operation
    {
        public string OperationName { get; set; }
    }
}