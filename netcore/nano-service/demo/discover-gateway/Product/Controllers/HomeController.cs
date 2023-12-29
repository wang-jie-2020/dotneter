using Microsoft.AspNetCore.Mvc;

namespace Product.Controllers
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