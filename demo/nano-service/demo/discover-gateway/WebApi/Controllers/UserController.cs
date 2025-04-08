using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("who are you");
        }

        [HttpPost]
        public IActionResult Post([FromForm] string userName)
        {
            return Content($"welcome,{userName}");
        }
    }
}
