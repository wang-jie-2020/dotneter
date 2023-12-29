using Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Consul
{
    public class ConsulServiceDiscoveryProvider : IConsulServiceDiscoveryProvider
    {
        ConsulServiceDiscoveryProviderOptions _options;
        IConsulClient _consul;
        ILogger _logger;
        Random _random = new Random();
        int currentIndex = 0;

        public ConsulServiceDiscoveryProvider(ConsulServiceDiscoveryProviderOptions options, IConsulClient consul, ILogger logger)
        {
            _options = options;
            _consul = consul;
            _logger = logger;
        }

        public virtual async Task<List<ConsulService>> GetServices()
        {
            ChechDisposed();

            var queryResult = await _consul.Health.Service(_options.ServiceName, _options.Tag, true);

            var services = new List<ConsulService>();

            foreach (var serviceEntry in queryResult.Response)
            {
                if (IsValid(serviceEntry))
                {
                    var nodes = await _consul.Catalog.Nodes();
                    if (nodes.Response == null)
                    {
                        services.Add(BuildService(serviceEntry, null));
                    }
                    else
                    {
                        var serviceNode = nodes.Response.FirstOrDefault(n => n.Address == serviceEntry.Service.Address);
                        services.Add(BuildService(serviceEntry, serviceNode));
                    }
                }
                else
                {
                    _logger.LogWarning($"Unable to use service Address: {serviceEntry.Service.Address} and Port: {serviceEntry.Service.Port}");
                }
            }

            return services.ToList();
        }
        public virtual async Task<ConsulService> GetService()
        {
            var services = await GetServices();

            if (_options.Mode == LoadBalancerMode.Random)
            {
                return services.ElementAtOrDefault(_random.Next(services.Count));//random
            }

            var index = Interlocked.Increment(ref currentIndex);
            if (index >= services.Count)
            {
                index = 0;
                Interlocked.Exchange(ref currentIndex, 0);
            }
            return services.ElementAtOrDefault(index);
        }

        private ConsulService BuildService(ServiceEntry serviceEntry, Node serviceNode)
        {
            return new ConsulService(serviceEntry.Service.ID,
                serviceEntry.Service.Service,
                serviceNode == null ? serviceEntry.Service.Address : serviceNode.Name,
                serviceEntry.Service.Port,
                serviceEntry.Service.Tags ?? Enumerable.Empty<string>());
        }

        private bool IsValid(ServiceEntry serviceEntry)
        {
            if (string.IsNullOrEmpty(serviceEntry.Service.Address) || serviceEntry.Service.Address.Contains("http://") || serviceEntry.Service.Address.Contains("https://") || serviceEntry.Service.Port <= 0)
            {
                return false;
            }

            return true;
        }

        protected void ChechDisposed()
        {
            if (_consul == null)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        public virtual void Dispose()
        {
            _consul?.Dispose();
            _consul = null;
        }
    }
    public class ConsulServiceDiscoveryProviderOptions
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 服务标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 均衡模式
        /// </summary>
        public LoadBalancerMode Mode { get; set; } = LoadBalancerMode.Random;
    }
}
