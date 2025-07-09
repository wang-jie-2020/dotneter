using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Framework3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MvcController: ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public MvcController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public object Get()
        {
            var mvcOptions = _serviceProvider.GetRequiredService<IOptions<MvcOptions>>().Value;
            return mvcOptions.Filters;
        }
        
        [HttpPost]
        [Authorize]
        public object Set()
        {
            return "Ok";
        }
    }
}