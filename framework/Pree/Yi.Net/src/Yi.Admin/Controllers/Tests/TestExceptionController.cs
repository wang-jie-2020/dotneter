using Microsoft.AspNetCore.Mvc;
using Yi.AspNetCore;
using Yi.System.Domains.Consts;

namespace Yi.Web.Controllers.Tests;

[ApiController]
[Route("test-exception")]
public class TestExceptionController : ControllerBase
{
    [HttpGet("runtimeException")]
    public void MapException()
    {
        throw new Exception();
    }

    [HttpGet("authorizationException")]
    public void MapAuthorizationException()
    {
        throw new UnauthorizedException();
    }

    [HttpGet("businessException")]
    public void MapBusinessException()
    {
        throw Oops.Oh(AccountConst.VerificationCode_Invalid);
    }
}