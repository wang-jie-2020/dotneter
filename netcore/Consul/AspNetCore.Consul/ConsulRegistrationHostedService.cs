using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Consul
{
    public class ConsulRegistrationHostedService : IHostedService
    {
        IEnumerable<IConsulServiceBuilder> builders;
        IOptionsMonitor<ServiceRegistrationOptions> optionsMonitor;
        IConsulClientFactory consulClientFactory;
        ILogger logger;
        CancellationTokenSource cancellationTokenSource;

        public ConsulRegistrationHostedService(IEnumerable<IConsulServiceBuilder> builders,
            IOptionsMonitor<ServiceRegistrationOptions> optionsMonitor,
            ILogger<ConsulRegistrationHostedService> logger,
            IConsulClientFactory consulClientFactory)
        {
            this.builders = builders;
            this.optionsMonitor = optionsMonitor;
            this.consulClientFactory = consulClientFactory;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            foreach (var builder in builders)
            {
                var options = optionsMonitor.Get(builder.Name);
                if (options.Services == null || options.Services.Count == 0) continue;

                var client = consulClientFactory.CreateClient(builder.Name);
                foreach (var service in options.Services)
                {
                    var serviceId = service.Id;
                    if (string.IsNullOrEmpty(serviceId))
                    {
                        serviceId = $"{service.Name}-{Guid.NewGuid()}";
                    }
                    var serviceName = service.Name;

                    var healthCheckUrl = service.HealthCheckUrl;
                    if (string.IsNullOrEmpty(healthCheckUrl))
                    {
                        var scheme = service.HealthCheckUseHttps ? "https" : "http";
                        healthCheckUrl = service.Port == 0 ? $"{scheme}://{service.Host}/" : $"{scheme}://{service.Host}:{service.Port}/";

                        if (!string.IsNullOrEmpty(service.HealthCheckPath))
                        {
                            healthCheckUrl += service.HealthCheckPath.TrimStart('/');
                        }
                        else
                        {
                            healthCheckUrl = healthCheckUrl.TrimEnd('/');
                        }
                    }

                    AgentServiceCheck check = new AgentServiceCheck()
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                        Timeout = TimeSpan.FromSeconds(5),
                        Interval = service.HealthCheckInterval ?? TimeSpan.FromSeconds(10),
                    };
                    if (service.HealthCheckUseGrpc)
                    {
                        check.GRPC = healthCheckUrl;
                        check.GRPCUseTLS = false;
                    }
                    else
                    {
                        check.HTTP = healthCheckUrl;
                    }

                    var result = await client.Agent.ServiceRegister(new AgentServiceRegistration()
                    {
                        Address = service.Host,
                        Port = service.Port,
                        ID = serviceId,
                        Name = serviceName,
                        Tags = service.Tags,
                        Check = check
                    });

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        logger.LogInformation($"service 【{serviceName}(ID:{serviceId})】 has registered successful");
                    }
                    else
                    {
                        logger.LogWarning($"service 【{serviceName}(ID:{serviceId})】 was fail to register[StatusCode:{result.StatusCode}]");
                    }

                    cancellationTokenSource.Token.Register(() =>
                    {
                        var _result = client.Agent.ServiceDeregister(serviceId).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (_result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            logger.LogInformation($"service 【{serviceName}(ID:{serviceId})】 has deregistered successful");
                        }
                        else
                        {
                            logger.LogWarning($"service 【{serviceName}(ID:{serviceId})】 was fail to deregister[StatusCode:{_result.StatusCode}]");
                        }
                        client.Dispose();
                    });
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();
            cancellationTokenSource.Cancel();
        }
    }
}
