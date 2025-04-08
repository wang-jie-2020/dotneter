using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResourceOwnerClient
{
    class Program
    {
        private static async Task Main()
        {
            var client = new HttpClient();

            //var discoveryClient = new DiscoveryClient(ids4Url) { Policy = { RequireHttps = false } };

            //GetDiscoveryDocumentAsync是IdentityModel中定义的扩展方法，简化EndPoint的调用过程
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001"); ;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.ReadKey();
                return;
            }

            /*
             * 默认是通过https访问（IdentityModel要求），若ids是http则需要配置策略
             * 注：localhost不会检查
             */
            //var disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            //{
            //    Address = "http://localhost:5000",
            //    Policy = new DiscoveryPolicy { RequireHttps = false }
            //});

            //请求Token，这里得到的就是accessToken
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client_resourceOwnerPassword",
                ClientSecret = "secret",
                UserName = "admin",
                Password = "admin"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.ReadKey();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            //请求api
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:6001/home/authorize");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }

            Console.ReadKey();
        }
    }
}
