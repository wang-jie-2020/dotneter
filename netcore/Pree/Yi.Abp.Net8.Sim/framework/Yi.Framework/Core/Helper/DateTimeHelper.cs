namespace Yi.Framework.Core.Helper;

public class DateTimeHelper
{
    /// <summary>
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static DateTime GetBeginTime(DateTime? dateTime, int days = 0)
    {
        if (dateTime == DateTime.MinValue || dateTime == null) return DateTime.Now.AddDays(days);
        return dateTime ?? DateTime.Now;
    }

    #region 毫秒转天时分秒

    /// <summary>
    ///     毫秒转天时分秒
    /// </summary>
    /// <param name="ms"></param>
    /// <returns></returns>
    public static string FormatTime(long ms)
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

        return string.Format("{0} 天 {1} 小时 {2} 分 {3} 秒", sDay, sHour, sMinute, sSecond);
    }

    #endregion

    #region 获取unix时间戳

    /// <summary>
    ///     获取unix时间戳
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static long GetUnixTimeStamp(DateTime dt)
    {
        var unixTime = ((DateTimeOffset)dt).ToUnixTimeMilliseconds();
        return unixTime;
    }

    #endregion

    #region 获取日期天的最小时间

    public static DateTime GetDayMinDate(DateTime dt)
    {
        var min = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        return min;
    }

    #endregion

    #region 获取日期天的最大时间

    public static DateTime GetDayMaxDate(DateTime dt)
    {
        var max = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
        return max;
    }

    #endregion

    #region 获取日期天的最大时间

    public static string FormatDateTime(DateTime? dt)
    {
        if (dt != null)
        {
            if (dt.Value.Year == DateTime.Now.Year)
                return dt.Value.ToString("MM-dd HH:mm");
            return dt.Value.ToString("yyyy-MM-dd HH:mm");
        }

        return string.Empty;
    }

    #endregion

    #region 时间戳转换

    /// <summary>
    ///     时间戳转本地时间-时间戳精确到秒
    /// </summary>
    public static DateTime ToLocalTimeDateBySeconds(long unix)
    {
        var dto = DateTimeOffset.FromUnixTimeSeconds(unix);
        return dto.ToLocalTime().DateTime;
    }

    /// <summary>
    ///     时间转时间戳Unix-时间戳精确到秒
    /// </summary>
    public static long ToUnixTimestampBySeconds(DateTime dt)
    {
        var dto = new DateTimeOffset(dt);
        return dto.ToUnixTimeSeconds();
    }

    /// <summary>
    ///     时间戳转本地时间-时间戳精确到毫秒
    /// </summary>
    public static DateTime ToLocalTimeDateByMilliseconds(long unix)
    {
        var dto = DateTimeOffset.FromUnixTimeMilliseconds(unix);
        return dto.ToLocalTime().DateTime;
    }

    /// <summary>
    ///     时间转时间戳Unix-时间戳精确到毫秒
    /// </summary>
    public static long ToUnixTimestampByMilliseconds(DateTime dt)
    {
        var dto = new DateTimeOffset(dt);
        return dto.ToUnixTimeMilliseconds();
    }

    #endregion
}