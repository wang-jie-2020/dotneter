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
        //[AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //if (User?.Identity?.IsAuthenticated ?? false)
            //{
            //    ViewBag.IdToken = await HttpContext.GetTokenAsync("id_token");
            //    ViewBag.AccessToken = await HttpContext.GetTokenAsync("access_token");

            //    return View();
            //}
            //else
            //{
            //    return Challenge(new[] { "oidc" });
            //}

            ViewBag.IdToken = await HttpContext.GetTokenAsync("id_token");
            ViewBag.AccessToken = await HttpContext.GetTokenAsync("access_token");

            return View();
        }

        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("https://localhost:6001/home/authorize");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }
    }
}
