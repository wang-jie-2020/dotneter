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
    [Authorize(policy: "apis.full.scope")]
    public class SecureController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "accessed into secure resource";
        }
    }
}
