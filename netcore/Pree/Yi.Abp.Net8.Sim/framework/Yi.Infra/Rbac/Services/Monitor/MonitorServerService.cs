﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Yi.Framework.Core.Helper;
using Yi.Infra.Rbac.IServices;

namespace Yi.Infra.Rbac.Services.Monitor;

public class MonitorServerService : ApplicationService, IMonitorServerService
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MonitorServerService(IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _hostEnvironment = hostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("monitor-server/info")]
    public object GetInfo()
    {
        var cpuNum = Environment.ProcessorCount;
        var computerName = Environment.MachineName;
        var osName = RuntimeInformation.OSDescription;
        var osArch = RuntimeInformation.OSArchitecture.ToString();
        var version = RuntimeInformation.FrameworkDescription;
        var appRAM = ((double)Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2") + " MB";
        var startTime = Process.GetCurrentProcess().StartTime.ToString("yyyy-MM-dd HH:mm:ss");
        var sysRunTime = ComputerHelper.GetRunTime();
        var serverIP = _httpContextAccessor.HttpContext.Connection.LocalIpAddress.MapToIPv4() + ":" +
                       _httpContextAccessor.HttpContext.Connection.LocalPort; //获取服务器IP

        var programStartTime = Process.GetCurrentProcess().StartTime;
        var programRunTime =
            FormatTime(
                long.Parse((DateTime.Now - programStartTime).TotalMilliseconds.ToString().Split('.')[0]));
        var data = new
        {
            cpu = ComputerHelper.GetComputerInfo(),
            disk = ComputerHelper.GetDiskInfos(),
            sys = new { cpuNum, computerName, osName, osArch, serverIP, runTime = sysRunTime },
            app = new
            {
                name = _hostEnvironment.EnvironmentName,
                rootPath = _hostEnvironment.ContentRootPath,
                webRootPath = _hostEnvironment.WebRootPath,
                version,
                appRAM,
                startTime,
                runTime = programRunTime,
                host = serverIP
            }
        };

        return data;
    }

    /// <summary>
    ///     毫秒转天时分秒
    /// </summary>
    /// <param name="ms"></param>
    /// <returns></returns>
    private static string FormatTime(long ms)
    {
        var ss = 1000;
        var mi = ss * 60;
        var hh = mi * 60;
        var dd = hh * 24;

        var day = ms / dd;
        var hour = (ms - day * dd) / hh;
        var minute = (ms - day * dd - hour * hh) / mi;
        var second = (ms - day * dd - hour * hh - minute * mi) / ss;
        var milliSecond = ms - day * dd - hour * hh - minute * mi - second * ss;

        var sDay = day < 10 ? "0" + day : "" + day; //天
        var sHour = hour < 10 ? "0" + hour : "" + hour; //小时
        var sMinute = minute < 10 ? "0" + minute : "" + minute; //分钟
        var sSecond = second < 10 ? "0" + second : "" + second; //秒
        var sMilliSecond = milliSecond < 10 ? "0" + milliSecond : "" + milliSecond; //毫秒
        sMilliSecond = milliSecond < 100 ? "0" + sMilliSecond : "" + sMilliSecond;

        return $"{sDay} 天 {sHour} 小时 {sMinute} 分 {sSecond} 秒";
    }
}