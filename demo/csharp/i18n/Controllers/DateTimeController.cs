using i18n.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace i18n.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DateTimeController : ControllerBase
{
    private readonly MySqlContext _mySqlContext;
    private readonly MsSqlContext _msSqlContext;
    private readonly OracleContext _oracleContext;

    public DateTimeController(MySqlContext mySqlContext, MsSqlContext msSqlContext, OracleContext oracleContext)
    {
        _mySqlContext = mySqlContext;
        _msSqlContext = msSqlContext;
        _oracleContext = oracleContext;
    }

    [HttpGet]
    public object InNewtonsoftJson()
    {
        var r = new DateTimeDto
        {
            DateTimeNow = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Local),
            DateTimeUtcNow = new DateTime(2025, 1, 10, 1, 0, 0, DateTimeKind.Utc),
            DateTimeUnspecified = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Unspecified),
        };

        var jsonLocal = JsonConvert.SerializeObject(r, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Local });
        var jsonUtc = JsonConvert.SerializeObject(r, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
        var jsonUnspecified = JsonConvert.SerializeObject(r, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.Unspecified });
        var jsonRoundtripKind = JsonConvert.SerializeObject(r, new JsonSerializerSettings() { DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind });

        /*
         *
         *
            "jsonLocal": "{
                \"DateTimeNow\":\"2025-01-10T09:00:00+08:00\",
                \"DateTimeUtcNow\":\"2025-01-10T09:00:00+08:00\",
                \"DateTimeUnspecified\":\"2025-01-10T09:00:00+08:00\"}",
            "jsonUtc": "{
                \"DateTimeNow\":\"2025-01-10T01:00:00Z\",
                \"DateTimeUtcNow\":\"2025-01-10T01:00:00Z\",
                \"DateTimeUnspecified\":\"2025-01-10T09:00:00Z\"}",
            "jsonUnspecified": "{
                \"DateTimeNow\":\"2025-01-10T09:00:00\",
                \"DateTimeUtcNow\":\"2025-01-10T01:00:00\",
                \"DateTimeUnspecified\":\"2025-01-10T09:00:00\"}",
            "jsonRoundtripKind": "{
                \"DateTimeNow\":\"2025-01-10T09:00:00+08:00\",
                \"DateTimeUtcNow\":\"2025-01-10T01:00:00Z\",
                \"DateTimeUnspecified\":\"2025-01-10T09:00:00\"}"
         *
         *
         */
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
        var r = new DateTimeDto
        {
            DateTimeNow = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Local),
            DateTimeUtcNow = new DateTime(2025, 1, 10, 1, 0, 0, DateTimeKind.Utc),
            DateTimeUnspecified = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Unspecified),
        };

        var cn = new
        {
            DateTimeNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeNow, TimeZoneConst.CN),
            DateTimeUtcNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeUtcNow, TimeZoneConst.CN),
            DateTimeUnspecified = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeUnspecified, TimeZoneConst.CN),
            DateTimeUnspecified2 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeUnspecified, TimeZoneInfo.Local.Id, TimeZoneConst.CN),
        };

        var us = new
        {
            DateTimeNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeNow, TimeZoneConst.US),
            DateTimeUtcNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeUtcNow, TimeZoneConst.US),
            DateTimeUnspecified = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeUnspecified, TimeZoneConst.US),
            DateTimeUnspecified2 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(r.DateTimeUnspecified, TimeZoneInfo.Local.Id, TimeZoneConst.US),
        };

        /*
         *  {
              "r": {
                "dateTimeNow": "2025-01-10T09:00:00",
                "dateTimeUtcNow": "2025-01-10T01:00:00",
                "dateTimeUnspecified": "2025-01-10T09:00:00"
              },
              "cn": {
                "dateTimeNow": "2025-01-10T09:00:00",
                "dateTimeUtcNow": "2025-01-10T09:00:00",
                "dateTimeUnspecified": "2025-01-10T09:00:00"
              },
              "us": {
                "dateTimeNow": "2025-01-09T20:00:00",
                "dateTimeUtcNow": "2025-01-09T20:00:00",
                "dateTimeUnspecified": "2025-01-09T20:00:00"
              }
            }
         */
        return new
        {
            r, cn, us
        };
    }
    
    [HttpGet]
    public object InSqlServer()
    {
        var r = new DateTimeDemo()
        {
            Time1 = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Local),
            Time2 = new DateTime(2025, 1, 10, 1, 0, 0, DateTimeKind.Utc),
            Time3 = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Unspecified),
            Time4 = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Unspecified).ToLocalTime(),
        };

        _msSqlContext.DateTimeDemos.Add(r);
        _msSqlContext.SaveChanges();

        var i = _msSqlContext.DateTimeDemos.OrderBy(t => t.Id).Last();

        var o = new
        {
            Time1 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time1 ?? throw new NullReferenceException(), TimeZoneConst.US),
            Time2 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time2 ?? throw new NullReferenceException(), TimeZoneConst.US),

            Time3 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time3 ?? throw new NullReferenceException(), TimeZoneConst.US),
            Time4 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time4 ?? throw new NullReferenceException(), TimeZoneConst.US),
        };

        return new { r, i, o };
    }

    [HttpGet]
    public object InMySql()
    {
        var r = new DateTimeDemo()
        {
            Time1 = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Local),
            Time2 = new DateTime(2025, 1, 10, 1, 0, 0, DateTimeKind.Utc),
            Time3 = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Unspecified),
            Time4 = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Unspecified).ToLocalTime(),
        };

        _mySqlContext.DateTimeDemos.Add(r);
        _mySqlContext.SaveChanges();

        var i = _mySqlContext.DateTimeDemos.OrderBy(t => t.Id).Last();

        var o = new
        {
            Time1 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time1 ?? throw new NullReferenceException(), TimeZoneConst.US),
            Time2 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time2 ?? throw new NullReferenceException(), TimeZoneConst.US),

            Time3 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time3 ?? throw new NullReferenceException(), TimeZoneConst.US),
            Time4 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time4 ?? throw new NullReferenceException(), TimeZoneConst.US),
        };

        return new { r, i, o };
    }
    
    [HttpGet]
    public object InOracle()
    {
        var r = new DateTimeDemo()
        {
            Time1 = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Local),
            Time2 = new DateTime(2025, 1, 10, 1, 0, 0, DateTimeKind.Utc),
            Time3 = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Unspecified),
            Time4 = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Unspecified).ToLocalTime(),
        };

        _oracleContext.DateTimeDemos.Add(r);
        _oracleContext.SaveChanges();

        var i = _oracleContext.DateTimeDemos.OrderBy(t => t.Id).Last();

        var o = new
        {
            Time1 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time1 ?? throw new NullReferenceException(), TimeZoneConst.US),
            Time2 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time2 ?? throw new NullReferenceException(), TimeZoneConst.US),

            Time3 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time3 ?? throw new NullReferenceException(), TimeZoneConst.US),
            Time4 = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(i.Time4 ?? throw new NullReferenceException(), TimeZoneConst.US),
        };

        return new { r, i, o };
    }
}