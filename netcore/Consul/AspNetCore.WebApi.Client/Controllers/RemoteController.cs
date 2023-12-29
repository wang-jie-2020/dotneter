using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Grpc.Net.ClientFactory;
using WebApi.Grpc;

namespace AspNetCore.WebApi.Client.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RemoteController : ControllerBase
    {
        private readonly ILogger<RemoteController> _logger;

        public RemoteController(ILogger<RemoteController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 使用Http方式调用远程接口
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Http(string name)
        {
            var httpClientFactory = HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient("http");//http是Startup中注册的http client名称
            var response = await httpClient.GetAsync($"/Remote?name={name}");
            return await response.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// 使用Grpc方式调用远程接口
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Grpc(string name)
        {
            var grpcClientFactory = HttpContext.RequestServices.GetRequiredService<GrpcClientFactory>();
            var grpcClient = grpcClientFactory.CreateClient<WebApiServer.WebApiServerClient>("grpc");//grpc是Startup中注册的grpc client名称
            var data = await grpcClient.SayAsync(new DataRequest() { Name = name });
            return data.Message;
        }
    }
}
