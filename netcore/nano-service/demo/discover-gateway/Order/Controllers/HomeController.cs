using Microsoft.AspNetCore.Mvc;

namespace Order.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("~/swagger/index.html");
        }
    }
}