using Microsoft.AspNetCore.Mvc;

namespace Order.Controllers
{
    [Route("api/order")]
    public class OrderController : Controller
    {
        [HttpGet]
        [Route("hello")]
        public IActionResult HelloOrder()
        {
            var ip = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort;
            return Content(ip);
        }
    }
}
