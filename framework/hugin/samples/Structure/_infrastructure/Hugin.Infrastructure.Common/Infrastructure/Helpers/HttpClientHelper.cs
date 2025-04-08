using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hugin.Infrastructure.Helpers
{
    /// <summary>
    /// Http请求的帮助类
    /// </summary>
    public class HttpClientHelper
    {
        private readonly string _baseUri;
        private readonly HttpClient _client;

        public HttpClientHelper(string baseUri = "")
        {
            _client = new HttpClient();
            _baseUri = baseUri;

            if (!string.IsNullOrEmpty(baseUri))
            {
                _client.BaseAddress = new Uri(baseUri);
            }
        }

        public HttpClientHelper(HttpClient client, string baseUri = "")
        {
            _client = client;
            _baseUri = baseUri;

            if (!string.IsNullOrEmpty(baseUri))
            {
                _client.BaseAddress = new Uri(baseUri);
            }
        }

        public HttpClientHelper(IHttpClientFactory clientFactory, string clientChannel = "Default", string baseUri = "")
        {
            _client = clientFactory.CreateClient(clientChannel);
            _baseUri = baseUri;

            if (!string.IsNullOrEmpty(baseUri))
                _client.BaseAddress = new Uri(baseUri);
        }

        /// <summary>
        /// Get或Delete
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        public async Task<string> Call(string url
            , HttpMethod method
            , IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var result = string.Empty;
            var rsp = await Execute(url, method, headers, null, HttpContentType.Json);
            using (StreamReader reader = new StreamReader(rsp, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// Post或Put
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="body">Json=字符串|对象,XForm=字符串|键值对,Form=键值对(key.._byte as byte[])</param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        public async Task<string> Call(string url
            , HttpMethod method
            , object body
            , HttpContentType contentType = HttpContentType.Json
            , IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var result = string.Empty;
            var rsp = await Execute(url, method, headers, body, contentType);
            using (StreamReader reader = new StreamReader(rsp, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// Get或Delete
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        public async Task<Stream> CallStream(string url
            , HttpMethod method
            , IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return await Execute(url, method, headers, null, HttpContentType.Json);
        }

        /// <summary>
        /// Post或Put
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="body">Json=字符串|对象,XForm=字符串|键值对,Form=键值对(key.._byte as byte[])</param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        public async Task<Stream> CallStream(string url
            , HttpMethod method
            , object body
            , HttpContentType contentType = HttpContentType.Json
            , IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return await Execute(url, method, headers, body, contentType);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="body">Json=字符串|对象,XForm=字符串|键值对,Form=键值对(key.._byte as byte[],_stream as stream)</param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        private async Task<Stream> Execute(string url
                        , HttpMethod method
                        , IEnumerable<KeyValuePair<string, string>> headers
                        , object body
                        , HttpContentType contentType)
        {
            if (url.StartsWith("https") || _baseUri.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var req = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = method
            };

            if (headers != null)
            {
                foreach (var header in headers.DefaultIfEmpty())
                {
                    req.Headers.Add(header.Key, header.Value);
                }
            }

            req.Content = _dealContent(body, contentType);

            var rsp = await _client.SendAsync(req);

            var result = await rsp.Content.ReadAsStreamAsync();   //rsp.Content.ReadAsStringAsync();
            if (!rsp.IsSuccessStatusCode)
            {
                throw new Exception(rsp.ReasonPhrase);
            }

            return result;
        }

        private HttpContent _dealContent(object body, HttpContentType contentType)
        {
            if (body == null)
                return null;

            if (contentType == HttpContentType.Json)
            {
                var data = _toString(body);
                if (data != null)
                {
                    return new StringContent(data, Encoding.UTF8, "application/json");
                }
                else
                {
                    data = JsonConvert.SerializeObject(body);
                    return new StringContent(data, Encoding.UTF8, "application/json");
                }
            }
            else if (contentType == HttpContentType.XForm)
            {
                var data = _toString(body);
                if (data != null)
                {
                    return new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");
                }
                else
                {
                    var dictionary = _toDictionary(body);
                    if (dictionary != null)
                    {
                        return new FormUrlEncodedContent(dictionary.Select(p => new KeyValuePair<string, string>(p.Key, p.Value.ToString())));

                        //data = dictionary.Select(m =>
                        //            System.Net.WebUtility.UrlEncode(m.Key) + "=" + System.Net.WebUtility.UrlEncode(m.Value.ToString())
                        //        ).DefaultIfEmpty().Aggregate((m, n) => m + "&" + n);
                    }

                    throw new NotSupportedException("support string or Dictionary<string,string>");
                }
            }
            else if (contentType == HttpContentType.Form)
            {
                var data = _toDictionary(body);
                if (data != null)
                {
                    var boundary = DateTime.Now.Ticks.ToString("X");
                    var formContent = new MultipartFormDataContent(boundary);

                    foreach (var item in data)
                    {
                        if (item.Key.EndsWith("_byte"))
                        {
                            var arrTemp = item.Key.Substring(0, item.Key.IndexOf("_byte")).Split('&');
                            var key = arrTemp[0];
                            var name = arrTemp.Length > 1 ? arrTemp[1] : "";

                            formContent.Add(new ByteArrayContent((byte[])item.Value), key, name);
                        }
                        else if (item.Key.EndsWith("_stream"))
                        {
                            var arrTemp = item.Key.Substring(0, item.Key.IndexOf("_stream")).Split('&');
                            var key = arrTemp[0];
                            var name = arrTemp.Length > 1 ? arrTemp[1] : "";

                            formContent.Add(new StreamContent((Stream)item.Value), key, name);
                        }
                        else
                        {
                            formContent.Add(new StringContent(item.Value.ToString()), item.Key);
                        }
                    }

                    return formContent;
                }

                throw new NotSupportedException("support Dictionary<string,object>");
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private string _toString(object obj)
        {
            try
            {
                var result = (string)Convert.ChangeType(obj, typeof(string));
                return result;
            }
            catch
            {
                // ignored
            }

            return null;
        }

        private Dictionary<string, object> _toDictionary(object obj)
        {
            try
            {
                var result = (Dictionary<string, object>)Convert.ChangeType(obj, typeof(Dictionary<string, object>));
                return result;
            }
            catch
            {
                // ignored
            }

            return null;
        }
    }

    /// <summary>
    /// 精简的content type
    /// </summary>
    public enum HttpContentType
    {
        Json,   //"application/json"
        Form,   //"multipart/form-data"
        XForm   //"application/x-www-form-urlencoded"
    }
}