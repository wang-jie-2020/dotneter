using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Grpc;

namespace AspNetCore.WebApi.Server
{
    public class WebApiImpl : WebApiServer.WebApiServerBase
    {
        ILogger<WebApiImpl> _logger;
        ServiceInfo _serviceInfo;

        public WebApiImpl(ILogger<WebApiImpl> logger, IOptions<ServiceInfo> options)
        {
            _logger = logger;
            _serviceInfo = options.Value;
        }
        
        /// <summary>
        /// grpc调用实现
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<DataReply> Say(DataRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"{nameof(WebApiImpl)} Say recieve:{request.Name}");
            return await Task.FromResult(new DataReply()
            {
                Message = $"data from grpc request=>{_serviceInfo.IPAddress}:{_serviceInfo.GrpcPort}"
            });
        }
    }
}
