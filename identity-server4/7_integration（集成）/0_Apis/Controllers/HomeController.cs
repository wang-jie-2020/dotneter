using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Apis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "hello there" + User?.Identity?.Name;
        }

        [HttpGet]
        [Route("authorize")]
        [Authorize]
        public IActionResult Authorize()
        {
            var claims = from c in User.Claims select new { c.Type, c.Value };
            return new JsonResult(claims);
        }

        [HttpGet]
        [Route("platform")]
        [Authorize("platform")]
        public string Platform()
        {
            return "accessed into platform resource";
        }

        [HttpGet]
        [Route("terminal")]
        [Authorize("terminal")]
        public string Terminal()
        {
            return "accessed into terminal resource";
        }

    }
}
