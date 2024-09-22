using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;

namespace Yi.Web.Controllers;

[ApiController]
[Route("dev-api")]
public class DevController : AbpController
{
    public DevController()
    {
    }

    [HttpGet("mvc-filters")]
    public object MvcOptions()
    {
        var mvc = LazyServiceProvider.LazyGetRequiredService<IOptions<MvcOptions>>().Value;
        return mvc.Filters;
    }
}