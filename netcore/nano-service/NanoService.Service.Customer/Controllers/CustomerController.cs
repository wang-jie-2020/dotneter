using Microsoft.AspNetCore.Mvc;
using NanoService.Service.Customer.Models;

namespace NanoService.Service.Customer.Controllers
{
    [Route("api/customer")]
    public class CustomerController : Controller
    {
        [HttpGet]
        public IActionResult Hello()
        {
            return HelloCustomer();
        }

        [HttpGet]
        [Route("hello")]
        public IActionResult HelloCustomer()
        {
            var connection = Request.HttpContext.Connection;
            var user = Request.HttpContext.User.Identity?.Name ?? "未知用户";
            var address = connection.LocalIpAddress?.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort;

            return Content($"{user}访问的是{address}");
        }

        /// <summary>
        /// 网关中显示的swagger的调用尝试方式一，体验感明显不好
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/customer/api/test-gateway-swagger")]
        public IActionResult HelloCustomerForGateway()
        {
            return HelloCustomer();
        }

        [HttpPost]
        [Route("/api/customer")]
        public ActionResult<AppCustomer> HelloNewCustomer([FromBody]AppCustomer customer)
        {
            return customer;
        }
    }
}
