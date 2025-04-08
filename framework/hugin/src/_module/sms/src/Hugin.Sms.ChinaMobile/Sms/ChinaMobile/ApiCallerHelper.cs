using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hugin.Sms.ChinaMobile
{
    /// <summary>
    /// ApiHelper 辅助进行api请求
    /// </summary>
    internal class ApiCallerHelper
    {
        public static async Task<T> Execute<T>(string url, HttpMethod method, string body = null,
            HttpContentType httpContentType = HttpContentType.Json)
        {
            T result;

            if (method == HttpMethod.Get)
            {
                result = await HttpGet<T>(url);
            }
            else
            {
                result = await HttpPost<T>(url, body, httpContentType);
            }

            return result;
        }

        private static async Task<T> HttpGet<T>(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Proxy = null;
            request.Accept = "text/html, application/xhtml+xml, */*";

            var responseContent = string.Empty;
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseContent = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        private static async Task<T> HttpPost<T>(string url, string body, HttpContentType httpContentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Proxy = null;
            request.Accept = "text/html, application/xhtml+xml, */*";
            switch (httpContentType)
            {
                case HttpContentType.Form:
                    request.ContentType = "multipart/form-data，";
                    break;
                case HttpContentType.xForm:
                    request.ContentType = "application/x-www-form-urlencoded";
                    break;
                case HttpContentType.Xml:
                    request.ContentType = "text/xml";
                    break;
                default:
                    request.ContentType = "application/json";
                    break;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);

            var responseContent = string.Empty;
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                responseContent = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<T>(responseContent);
        }
    }

    internal enum HttpMethod
    {
        Post,
        Get
    }

    internal enum HttpContentType
    {
        Json,
        Form,
        xForm,
        Xml
    }
}