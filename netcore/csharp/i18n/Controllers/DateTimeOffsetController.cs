using i18n.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace i18n.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DateTimeOffsetController : ControllerBase
{
    private readonly MySqlContext _mySqlContext;
    private readonly MsSqlContext _msSqlContext;
    private readonly OracleContext _oracleContext;

    public DateTimeOffsetController(MySqlContext mySqlContext, MsSqlContext msSqlContext, OracleContext oracleContext)
    {
        _mySqlContext = mySqlContext;
        _msSqlContext = msSqlContext;
        _oracleContext = oracleContext;
    }

    [HttpGet]
    public object InNewtonsoftJson()
    {
        //DateTimeOffset = DateTime + TimeZone
        //DateTime -> DateTimeOffset , Local and Unspecified are both treated as Local
        var r = new DateTimeOffsetDto()
        {
            DateTimeOffsetNow = DateTimeOffset.Now,
            DateTimeOffsetUtcNow = DateTimeOffset.UtcNow
        };

        //DateTimeOffset 不会和DateTime一样,被序列化参数影响
        var jsonLocal = JsonConvert.SerializeObject(r, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Local });
        var jsonUtc = JsonConvert.SerializeObject(r, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
        var jsonUnspecified = JsonConvert.SerializeObject(r, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified });
        var jsonRoundtripKind = JsonConvert.SerializeObject(r, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind });

        return new
        {
            jsonLocal,
            jsonUtc,
            jsonUnspecified,
            jsonRoundtripKind
        };
    }
    
    [HttpGet]
    public object ConvertTimeZone()
    {
        var r = new DateTimeOffsetDto()
        {
            DateTimeOffsetNow = DateTimeOffset.Now,
            DateTimeOffsetUtcNow = DateTimeOffset.UtcNow
        };

        var cn = new
        {
            DateTimeOffsetNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeOffsetNow, TimeZoneConst.CN),
            DateTimeOffsetUtcNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeOffsetUtcNow, TimeZoneConst.CN),
        };

        var us = new
        {
            DateTimeOffsetNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeOffsetNow, TimeZoneConst.US),
            DateTimeOffsetUtcNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeOffsetUtcNow, TimeZoneConst.US),
        };

        return new
        {
            r, cn, us
        };
    }
    
    [HttpGet]
    public object InSqlServer()
    {
        var r = new DateTimeOffsetDemo()
        {
            Time11 = DateTimeOffset.Now,
            Time12 = DateTimeOffset.UtcNow,
            Time13 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.Now, TimeZoneConst.US),
            Time14 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, TimeZoneConst.US)
        };

        _msSqlContext.DateTimeOffsetDemos.Add(r);
        _msSqlContext.SaveChanges();

        var i = _msSqlContext.DateTimeOffsetDemos.OrderBy(t => t.Id).Last();

        var o = new
        {
            Time11 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time11 ?? throw new NullReferenceException(), TimeZoneConst.JP),
            Time12 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time12 ?? throw new NullReferenceException(), TimeZoneConst.JP),

            Time13 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time13 ?? throw new NullReferenceException(), TimeZoneConst.JP),
            Time14 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time14 ?? throw new NullReferenceException(), TimeZoneConst.JP),
        };

        return new { r, i, o };
    }

    [HttpGet]
    public object InMySql()
    {
        var r = new DateTimeOffsetDemo()
        {
            Time11 = DateTimeOffset.Now,
            Time12 = DateTimeOffset.UtcNow,
            Time13 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.Now, TimeZoneConst.US),
            Time14 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, TimeZoneConst.US)
        };

        _mySqlContext.DateTimeOffsetDemos.Add(r);
        _mySqlContext.SaveChanges();

        var i = _mySqlContext.DateTimeOffsetDemos.OrderBy(t => t.Id).Last();

        var o = new
        {
            Time11 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time11 ?? throw new NullReferenceException(), TimeZoneConst.JP),
            Time12 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time12 ?? throw new NullReferenceException(), TimeZoneConst.JP),

            Time13 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time13 ?? throw new NullReferenceException(), TimeZoneConst.JP),
            Time14 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time14 ?? throw new NullReferenceException(), TimeZoneConst.JP),
        };

        return new { r, i, o };
    }
    
    [HttpGet]
    public object InOracle()
    {
        var r = new DateTimeOffsetDemo()
        {
            Time11 = DateTimeOffset.Now,
            Time12 = DateTimeOffset.UtcNow,
            Time13 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.Now, TimeZoneConst.US),
            Time14 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, TimeZoneConst.US)
        };

        _oracleContext.DateTimeOffsetDemos.Add(r);
        _oracleContext.SaveChanges();

        var i = _oracleContext.DateTimeOffsetDemos.OrderBy(t => t.Id).Last();

        var o = new
        {
            Time11 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time11 ?? throw new NullReferenceException(), TimeZoneConst.JP),
            Time12 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time12 ?? throw new NullReferenceException(), TimeZoneConst.JP),

            Time13 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time13 ?? throw new NullReferenceException(), TimeZoneConst.JP),
            Time14 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time14 ?? throw new NullReferenceException(), TimeZoneConst.JP),
        };

        return new { r, i, o };
    }
}