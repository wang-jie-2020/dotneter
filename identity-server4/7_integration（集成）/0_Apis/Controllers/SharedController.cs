using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(policy: "SharedScope")]
    public class SharedController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "accessed into shared resource";
        }
    }
}
