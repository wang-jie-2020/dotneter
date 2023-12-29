using Microsoft.AspNetCore.Mvc;

namespace Custom.Controllers
{
    [Route("api/custom")]
    public class CustomController : Controller
    {
        [HttpGet]
        [Route("hello")]
        public IActionResult HelloCustom()
        {
            var ip = Request.HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString() + ":" + Request.HttpContext.Connection.LocalPort;
            return Content(ip);
        }
    }
}
