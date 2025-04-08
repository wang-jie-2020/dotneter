using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.IdToken = await HttpContext.GetTokenAsync("id_token");
            ViewBag.AccessToken = await HttpContext.GetTokenAsync("access_token");

            return View();
        }
    }
}
