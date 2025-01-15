using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace i18n.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TimeZoneController : ControllerBase
{
    private readonly ILogger<TimeZoneController> _logger;

    public TimeZoneController(ILogger<TimeZoneController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public object Cultures()
    {
        var culture = CultureInfo.CurrentCulture;

        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        return CultureInfo.CurrentCulture;
    }
    
    
    
    [HttpGet]
    public object TimeZones()
    {
        var local = TimeZoneInfo.Local;
        var cn = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneConst.CN);
        var us = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneConst.US);
        var jp = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneConst.JP);

        return new
        {
            local,
            cn,
            us,
            jp
        };
    }
}