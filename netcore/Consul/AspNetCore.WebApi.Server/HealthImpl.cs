using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Health.V1;

namespace AspNetCore.WebApi.Server
{
    public class HealthImpl : Health.HealthBase
    {
        ILogger<HealthImpl> _logger;

        public HealthImpl(ILogger<HealthImpl> logger)
        {
            _logger = logger;
        }

        public override async Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"{nameof(HealthImpl)} Check:{ request.Service}");
            return await Task.FromResult(new HealthCheckResponse()
            {
                Status = HealthCheckResponse.Types.ServingStatus.Serving
            });
        }

        public override async Task Watch(HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context)
        {
            _logger.LogInformation($"{nameof(HealthImpl)} Check");
            await responseStream.WriteAsync(new HealthCheckResponse()
            {
                Status = HealthCheckResponse.Types.ServingStatus.Serving
            });
        }
    }
}
