﻿using IPTools.Core;
using Microsoft.AspNetCore.Http;
using UAParser;
using Yi.Framework.Utils;

namespace Yi.System.Entities;

[SugarTable("Sys_LoginLog")]
[SugarIndex($"index_{nameof(LoginUser)}", nameof(LoginUser), OrderByType.Asc)]
public class LoginLogEntity : Entity<long>
{
    /// <summary>
    ///     登录用户
    /// </summary>
    [SugarColumn(ColumnName = "LoginUser")]
    public string? LoginUser { get; set; }

    /// <summary>
    ///     登录地点
    /// </summary>
    [SugarColumn(ColumnName = "LoginLocation")]
    public string? LoginLocation { get; set; }

    /// <summary>
    ///     登录Ip
    /// </summary>
    [SugarColumn(ColumnName = "LoginIp")]
    public string? LoginIp { get; set; }

    /// <summary>
    ///     浏览器
    /// </summary>
    [SugarColumn(ColumnName = "Browser")]
    public string? Browser { get; set; }

    /// <summary>
    ///     操作系统
    /// </summary>
    [SugarColumn(ColumnName = "Os")]
    public string? Os { get; set; }

    /// <summary>
    ///     登录信息
    /// </summary>
    [SugarColumn(ColumnName = "LogMsg")]
    public string? LogMsg { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }
    
    public LoginLogEntity GetInfoByHttpContext(HttpContext context)
    {
        ClientInfo GetClientInfo(HttpContext context)
        {
            var str = context.GetUserAgent();
            var uaParser = Parser.GetDefault();
            ClientInfo c;
            try
            {
                c = uaParser.Parse(str);
            }
            catch
            {
                c = new ClientInfo("null", new OS("null", "null", "null", "null", "null"),
                    new Device("null", "null", "null"), new UserAgent("null", "null", "null", "null"));
            }

            return c;
        }

        var ipAddr = context.GetClientIp();
        IpInfo location;
        if (ipAddr == "127.0.0.1")
            location = new IpInfo { Province = "本地", City = "本机" };
        else
            location = IpTool.Search(ipAddr);
        var clientInfo = GetClientInfo(context);
        LoginLogEntity entity = new()
        {
            Browser = clientInfo.Device.Family,
            Os = clientInfo.OS.ToString(),
            LoginIp = ipAddr,
            LoginLocation = location.Province + "-" + location.City
        };
        return entity;
    }
}