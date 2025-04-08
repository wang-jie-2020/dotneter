using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityModel;

namespace Hugin.BookStore
{
    public class BookStoreClientService : ITransientDependency
    {
        private readonly IIdentityModelAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
        private readonly IHealthCheckService _healthCheckService;

        public BookStoreClientService(IIdentityModelAuthenticationService authenticationService,
            IConfiguration configuration,
            IHealthCheckService healthCheckService)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
            _healthCheckService = healthCheckService;
        }

        public async Task RunAsync()
        {
            await DynamicProxiesAsync();
            await HttpClientAndIdentityModelAuthenticationServiceAsync();
            await ManuallyAsync();
        }

        /* Shows how to perform an HTTP request to the API using ABP's dynamic c# proxy
         * feature. It is just simple as calling a local service method.
         * Authorization and HTTP request details are handled by the ABP framework.
         */
        private async Task DynamicProxiesAsync()
        {
            Console.WriteLine();
            Console.WriteLine($"*****ABP动态代理*****");

            var result = await _healthCheckService.HeathCheck();
            Console.WriteLine("Result: " + result);

            result = await _healthCheckService.Authorize();
            Console.WriteLine("Result (authorized): " + result);
        }

        /* Shows how to use HttpClient to perform a request to the HTTP API.
         * It uses ABP's IIdentityModelAuthenticationService to simplify obtaining access tokens.
         */
        private async Task HttpClientAndIdentityModelAuthenticationServiceAsync()
        {
            Console.WriteLine();
            Console.WriteLine($"*****ABP集成*****");

            var accessToken = await _authenticationService.GetAccessTokenAsync(
                new IdentityClientConfiguration(
                    _configuration["IdentityClients:Default:Authority"],
                    _configuration["IdentityClients:Default:Scope"],
                    _configuration["IdentityClients:Default:ClientId"],
                    _configuration["IdentityClients:Default:ClientSecret"],
                    _configuration["IdentityClients:Default:GrantType"],
                    _configuration["IdentityClients:Default:UserName"],
                    _configuration["IdentityClients:Default:UserPassword"]
                )
            );

            using (var httpClient = new HttpClient())
            {
                httpClient.SetBearerToken(accessToken);

                var url = _configuration["RemoteServices:BookStore:BaseUrl"] + "api/healthCheck/authorize";

                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseString = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine("Result (authorized): " + responseString);
                }
                else
                {
                    throw new Exception("Remote server returns error code: " + responseMessage.StatusCode);
                }
            }
        }

        /* Shows how to use HttpClient to perform a request to the HTTP API.
         * It obtains access token using IdentityServer's API. See its documentation:
         * https://identityserver4.readthedocs.io/en/latest/quickstarts/2_resource_owner_passwords.html
         */
        private async Task ManuallyAsync()
        {
            Console.WriteLine();
            Console.WriteLine($"*****不集成*****");

            // discover endpoints from metadata
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_configuration["IdentityClients:Default:Authority"]);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _configuration["IdentityClients:Default:ClientId"],
                ClientSecret = _configuration["IdentityClients:Default:ClientSecret"],
                UserName = _configuration["IdentityClients:Default:UserName"],
                Password = _configuration["IdentityClients:Default:UserPassword"],
                Scope = _configuration["IdentityClients:Default:Scope"]
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            using (var httpClient = new HttpClient())
            {
                httpClient.SetBearerToken(tokenResponse.AccessToken);

                var url = _configuration["RemoteServices:BookStore:BaseUrl"] + "api/healthCheck/authorize";

                var responseMessage = await httpClient.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseString = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: " + responseString);
                }
                else
                {
                    throw new Exception("Remote server returns error code: " + responseMessage.StatusCode);
                }
            }
        }
    }
}