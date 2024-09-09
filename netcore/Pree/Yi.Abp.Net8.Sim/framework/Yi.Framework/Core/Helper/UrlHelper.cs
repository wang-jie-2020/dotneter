using System.Text;
using System.Web;

namespace Yi.Framework.Core.Helper;

public class UrlHelper
{
    /// <summary>
    ///     UrlEncode编码
    /// </summary>
    /// <param name="url">url</param>
    /// <returns></returns>
    public static string UrlEncode(string url)
    {
        return HttpUtility.UrlEncode(url, Encoding.UTF8);
    }

    /// <summary>
    ///     UrlEncode解码
    /// </summary>
    /// <param name="data">数据</param>
    /// <returns></returns>
    public static string UrlDecode(string data)
    {
        return HttpUtility.UrlDecode(data, Encoding.UTF8);
    }
}