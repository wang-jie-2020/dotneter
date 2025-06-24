using Microsoft.AspNetCore.Mvc;
using Yi.AspNetCore.Mvc;
using Yi.Framework.Core.Abstractions;
using Yi.System.Domains;

namespace Yi.Web.Controllers.Tests;

[ApiController]
[Route("test-ajax-result")]
public class TestAjaxResultController : BaseController
{
    [HttpGet("success")]
    public AjaxResult MapSuccess()
    {
        return AjaxResult.Success();
    }

    [HttpGet("success2")]
    public AjaxResult MapSuccess2()
    {
        return AjaxResult.Success(new LoginEventArgs());
    }

    [HttpGet("success3")]
    public AjaxResult<LoginEventArgs> MapSuccess3()
    {
        return AjaxResult<LoginEventArgs>.Success(new LoginEventArgs());
    }

    [HttpGet("error")]
    public AjaxResult MapError()
    {
        return AjaxResult.Error();
    }

    [HttpGet("error2")]
    public AjaxResult MapError2()
    {
        return AjaxResult.Error("error", "details");
    }
}