using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Volo.Abp.AspNetCore.Mvc;

namespace Hugin.BookStore.Controllers
{
    public class HomeController : AbpController
    {
        private readonly IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public ActionResult Index()
        {
            if (_environment.IsProduction())
            {
                return new ContentResult
                {
                    Content = "Host Start"
                };
            }

            return Redirect("~/swagger");
        }
    }
}
