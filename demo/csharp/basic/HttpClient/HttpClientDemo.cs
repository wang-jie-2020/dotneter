using System.Net;

namespace basic.HttpClient
{
    internal class HttpClientDemo
    {
        async Task Method1()
        {
            {
                var httpClient = new System.Net.Http.HttpClient();
                await httpClient.GetAsync("https://www.baidu.com");
            }

            {
                var handler = new HttpClientHandler
                {
                    Proxy = new WebProxy("127.0.0.1", 8888)
                };
                var httpClient = new System.Net.Http.HttpClient(handler);
                await httpClient.GetAsync("https://www.baidu.com");
            }

            {
                var handler = new HttpClientHandler
                {
                    Proxy = new WebProxy("127.0.0.1", 8888)
                };

                var httpClient = HttpClientFactory.Create(handler);
                await httpClient.GetAsync("https://www.baidu.com");
            }

            {
                WebRequest.DefaultWebProxy = new WebProxy("127.0.0.1", 8888);
                var httpClient = new System.Net.Http.HttpClient();
                await httpClient.GetAsync("https://www.baidu.com");
            }
        }
    }
}