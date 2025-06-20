using IPTools.Core;
using Microsoft.AspNetCore.Http;
using UAParser;
using Yi.AspNetCore.Mvc;

namespace Yi.System.Domains;

public class LoginEventArgs
{
    public Guid UserId { get; set; }

    public string UserName { get; set; }

    public DateTime CreationTime { get; set; }

    /// <summary>
    ///     登录地点
    /// </summary>
    public string? LoginLocation { get; set; }

    /// <summary>
    ///     登录Ip
    /// </summary>
    public string? LoginIp { get; set; }

    /// <summary>
    ///     浏览器
    /// </summary>
    public string? Browser { get; set; }

    /// <summary>
    ///     操作系统
    /// </summary>

    public string? Os { get; set; }

    /// <summary>
    ///     登录信息
    /// </summary>
    public string? LogMsg { get; set; }

    public LoginEventArgs GetInfoByHttpContext(HttpContext context)
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
        LoginEventArgs entity = new()
        {
            Browser = clientInfo.Device.Family,
            Os = clientInfo.OS.ToString(),
            LoginIp = ipAddr,
            LoginLocation = location.Province + "-" + location.City
        };

        return entity;
    }
}