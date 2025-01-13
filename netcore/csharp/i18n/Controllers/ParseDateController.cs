using Microsoft.AspNetCore.Mvc;

namespace i18n.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ParseDateController : ControllerBase
{
    public ParseDateController()
    {
    }

    [HttpGet]
    public object InputDateTime([FromQuery] DateTimeDto input)
    {
        return input;
    }

    [HttpGet]
    public object InputDateTimeOffset([FromQuery] DateTimeOffsetDto input)
    {
        return input;
    }
}