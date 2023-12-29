using Microsoft.AspNetCore.Mvc;

namespace Product.Controllers
{
    [Route("api/product")]
    public class ProductController : Controller
    {
        [HttpGet]
        [Route("hello")]
        public IActionResult HelloProduct()
        {
            var ip = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort;
            return Content(ip);
        }
    }
}
