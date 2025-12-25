using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace I18n.LocalizationExtensions.Samples.Controllers;

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

public class InputDateTime
{
    [JsonProperty(Order = 1)]
    public DateTime DateTime1 { get; set; }
    
    [JsonProperty(Order = 2)]
    public DateTime DateTime2 { get; set; }
    
    [JsonProperty(Order = 3)]
    public DateTime DateTime3 { get; set; }

}