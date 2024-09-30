using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Authorization;
using Yi.AspNetCore.System;
using Yi.AspNetCore.System.Entities;
using Yi.AspNetCore.System.Events;

namespace Yi.Web.Controllers;

[ApiController]
[Route("dev-api")]
public class DevController : AbpController
{
    public DevController()
    {
    }

    [HttpGet("mvc")]
    public object MvcOptions()
    {
        var mvc = LazyServiceProvider.LazyGetRequiredService<IOptions<MvcOptions>>().Value;
        return new
        {
            mvc.Filters,
            mvc.Conventions,
            mvc.ModelBinderProviders,
        };
    }

    [HttpGet("success")]
    public AjaxResult MapSuccess()
    {
        return AjaxResult.Success();
    }

    [HttpGet("success2")]
    public AjaxResult<LoginEventArgs> MapSuccess2()
    {
        return AjaxResult<LoginEventArgs>.Success(new LoginEventArgs());
    }

    [HttpGet("error")]
    public AjaxResult MapError()
    {
        return AjaxResult.Error("123123123");
    }

    [HttpGet("exception")]
    public void MapException()
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("authorizationException")]
    public void MapAuthorizationException()
    {
        throw new AbpAuthorizationException();
    }
}