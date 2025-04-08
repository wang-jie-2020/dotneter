using Microsoft.AspNetCore.Mvc;

namespace i18n.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ParseDateController : ControllerBase
{
    public ParseDateController()
    {
    }
    
    [HttpPost]
    public object InputQueryDateTime([FromQuery] InputDateTime input)
    {
        return input;
    }
    
    [HttpPost]
    public object InputFormDateTime([FromForm] InputDateTime input)
    {
        return input;
    }

    [HttpPost]
    public object InputBodyDateTime([FromBody] InputDateTime input)
    {
        return input;
    }
}