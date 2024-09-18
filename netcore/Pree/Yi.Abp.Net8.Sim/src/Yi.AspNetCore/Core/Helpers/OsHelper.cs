using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Yi.AspNetCore.Core.Helpers;

public class OsHelper
{
    /// <summary>
    ///     将object转换为long，若转换失败，则返回0。不抛出异常。
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static long ParseToLong(object obj)
    {
        try
        {
            return long.Parse(obj.ToString());
        }
        catch
        {
            return 0L;
        }
    }

    /// <summary>
    ///     将string转换为DateTime，若转换失败，则返回日期最小值。不抛出异常。
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private static DateTime ParseToDateTime(string str)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(str)) return DateTime.MinValue;
            if (str.Contains("-") || str.Contains("/")) return DateTime.Parse(str);

            var length = str.Length;
            switch (length)
            {
                case 4:
                    return DateTime.ParseExact(str, "yyyy", CultureInfo.CurrentCulture);
                case 6:
                    return DateTime.ParseExact(str, "yyyyMM", CultureInfo.CurrentCulture);
                case 8:
                    return DateTime.ParseExact(str, "yyyyMMdd", CultureInfo.CurrentCulture);
                case 10:
                    return DateTime.ParseExact(str, "yyyyMMddHH", CultureInfo.CurrentCulture);
                case 12:
                    return DateTime.ParseExact(str, "yyyyMMddHHmm", CultureInfo.CurrentCulture);
                case 14:
                    return DateTime.ParseExact(str, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                default:
                    return DateTime.ParseExact(str, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
            }
        }
        catch
        {
            return DateTime.MinValue;
        }
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
    
    private static double ParseToDouble(object obj)
    {
        try
        {
            return double.Parse(obj.ToString());
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    ///     内存使用情况
    /// </summary>
    /// <returns></returns>
    public static MemoryMetrics GetComputerInfo()
    {
        try
        {
            MemoryMetricsClient client = new();
            var memoryMetrics = IsUnix() ? client.GetUnixMetrics() : client.GetWindowsMetrics();

            memoryMetrics.FreeRam = Math.Round(memoryMetrics.Free / 1024, 2) + "GB";
            memoryMetrics.UsedRam = Math.Round(memoryMetrics.Used / 1024, 2) + "GB";
            memoryMetrics.TotalRAM = Math.Round(memoryMetrics.Total / 1024, 2) + "GB";
            memoryMetrics.RAMRate = Math.Ceiling(100 * memoryMetrics.Used / memoryMetrics.Total) + "%";
            memoryMetrics.CPURate = Math.Ceiling(ParseToDouble(GetCPURate())) + "%";
            return memoryMetrics;
        }
        catch (Exception ex)
        {
            Console.WriteLine("获取内存使用出错，msg=" + ex.Message + "," + ex.StackTrace);
        }

        return new MemoryMetrics();
    }

    /// <summary>
    ///     获取内存大小
    /// </summary>
    /// <returns></returns>
    public static List<DiskInfo> GetDiskInfos()
    {
        List<DiskInfo> diskInfos = new();

        if (IsUnix())
        {
            try
            {
                var output = Bash("df -m / | awk '{print $2,$3,$4,$5,$6}'");
                var arr = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 0) return diskInfos;

                var rootDisk = arr[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                if (rootDisk == null || rootDisk.Length == 0) return diskInfos;
                DiskInfo diskInfo = new()
                {
                    DiskName = "/",
                    TotalSize = long.Parse(rootDisk[0]) / 1024,
                    Used = long.Parse(rootDisk[1]) / 1024,
                    AvailableFreeSpace = long.Parse(rootDisk[2]) / 1024,
                    AvailablePercent = decimal.Parse(rootDisk[3].Replace("%", ""))
                };
                diskInfos.Add(diskInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取磁盘信息出错了" + ex.Message);
            }
        }
        else
        {
            var driv = DriveInfo.GetDrives();
            foreach (var item in driv)
                try
                {
                    var obj = new DiskInfo
                    {
                        DiskName = item.Name,
                        TypeName = item.DriveType.ToString(),
                        TotalSize = item.TotalSize / 1024 / 1024 / 1024,
                        AvailableFreeSpace = item.AvailableFreeSpace / 1024 / 1024 / 1024
                    };
                    obj.Used = obj.TotalSize - obj.AvailableFreeSpace;
                    obj.AvailablePercent = decimal.Ceiling(obj.Used / (decimal)obj.TotalSize * 100);
                    diskInfos.Add(obj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("获取磁盘信息出错了" + ex.Message);
                }
        }

        return diskInfos;
    }

    public static bool IsUnix()
    {
        var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
                     RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        return isUnix;
    }

    public static string GetCPURate()
    {
        string cpuRate;
        if (IsUnix())
        {
            var output = Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
            cpuRate = output.Trim();
        }
        else
        {
            var output = Cmd("wmic", "cpu get LoadPercentage");
            cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
        }

        return cpuRate;
    }

    /// <summary>
    ///     获取系统运行时间
    /// </summary>
    /// <returns></returns>
    public static string GetRunTime()
    {
        var runTime = string.Empty;
        try
        {
            if (IsUnix())
            {
                var output = Bash("uptime -s").Trim();
                runTime = FormatTime(ParseToLong((DateTime.Now - ParseToDateTime(output))
                    .TotalMilliseconds.ToString().Split('.')[0]));
            }
            else
            {
                var output = Cmd("wmic", "OS get LastBootUpTime/Value");
                var outputArr = output.Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
                if (outputArr.Length == 2)
                    runTime = FormatTime(ParseToLong(
                        (DateTime.Now - ParseToDateTime(outputArr[1].Split('.')[0])).TotalMilliseconds.ToString()
                        .Split('.')[0]));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("获取runTime出错" + ex.Message);
        }

        return runTime;
    }
    
    /// <summary>
    ///     获取当前IP地址
    /// </summary>
    /// <param name="preferredNetworks"></param>
    /// <returns></returns>
    public static string GetCurrentIp(string preferredNetworks)
    {
        var instanceIp = "127.0.0.1";

        try
        {
            // 获取可用网卡
            var nics = NetworkInterface.GetAllNetworkInterfaces()
                ?.Where(network => network.OperationalStatus == OperationalStatus.Up);

            // 获取所有可用网卡IP信息
            var ipCollection = nics?.Select(x => x.GetIPProperties())?.SelectMany(x => x.UnicastAddresses);

            if (ipCollection is null) return instanceIp;

            foreach (var ipadd in ipCollection)
                if (!IPAddress.IsLoopback(ipadd.Address) && ipadd.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (string.IsNullOrEmpty(preferredNetworks))
                    {
                        instanceIp = ipadd.Address.ToString();
                        break;
                    }

                    if (!ipadd.Address.ToString().StartsWith(preferredNetworks)) continue;
                    instanceIp = ipadd.Address.ToString();
                    break;
                }
        }
        catch
        {
            // ignored
        }

        return instanceIp;
    }
    
    /// <summary>
    ///     linux 系统命令
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static string Bash(string command)
    {
        var escapedArgs = command.Replace("\"", "\\\"");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        var result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        process.Dispose();
        return result;
    }

    /// <summary>
    ///     windows系统命令
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string Cmd(string fileName, string args)
    {
        var output = string.Empty;

        var info = new ProcessStartInfo();
        info.FileName = fileName;
        info.Arguments = args;
        info.RedirectStandardOutput = true;

        using (var process = Process.Start(info))
        {
            output = process.StandardOutput.ReadToEnd();
        }

        return output;
    }
}

/// <summary>
///     内存信息
/// </summary>
public class MemoryMetrics
{
    [JsonIgnore] public double Total { get; set; }

    [JsonIgnore] public double Used { get; set; }

    [JsonIgnore] public double Free { get; set; }

    public string UsedRam { get; set; }

    /// <summary>
    ///     CPU使用率%
    /// </summary>
    public string CPURate { get; set; }

    /// <summary>
    ///     总内存 GB
    /// </summary>
    public string TotalRAM { get; set; }

    /// <summary>
    ///     内存使用率 %
    /// </summary>
    public string RAMRate { get; set; }

    /// <summary>
    ///     空闲内存
    /// </summary>
    public string FreeRam { get; set; }
}

public class DiskInfo
{
    /// <summary>
    ///     磁盘名
    /// </summary>
    public string DiskName { get; set; }

    public string TypeName { get; set; }
    public long TotalFree { get; set; }
    public long TotalSize { get; set; }

    /// <summary>
    ///     已使用
    /// </summary>
    public long Used { get; set; }

    /// <summary>
    ///     可使用
    /// </summary>
    public long AvailableFreeSpace { get; set; }

    public decimal AvailablePercent { get; set; }
}

public class MemoryMetricsClient
{
    #region 获取内存信息

    /// <summary>
    ///     windows系统获取内存信息
    /// </summary>
    /// <returns></returns>
    public MemoryMetrics GetWindowsMetrics()
    {
        var output = OsHelper.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
        var metrics = new MemoryMetrics();
        var lines = output.Trim().Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length <= 0) return metrics;

        var freeMemoryParts = lines[0].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);
        var totalMemoryParts = lines[1].Split('=', (char)StringSplitOptions.RemoveEmptyEntries);

        metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
        metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0); //m
        metrics.Used = metrics.Total - metrics.Free;

        return metrics;
    }

    /// <summary>
    ///     Unix系统获取
    /// </summary>
    /// <returns></returns>
    public MemoryMetrics GetUnixMetrics()
    {
        var output = OsHelper.Bash("free -m | awk '{print $2,$3,$4,$5,$6}'");
        var metrics = new MemoryMetrics();
        var lines = output.Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length <= 0) return metrics;

        if (lines != null && lines.Length > 0)
        {
            var memory = lines[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
            if (memory.Length >= 3)
            {
                metrics.Total = double.Parse(memory[0]);
                metrics.Used = double.Parse(memory[1]);
                metrics.Free = double.Parse(memory[2]); //m
            }
        }

        return metrics;
    }

    #endregion
}