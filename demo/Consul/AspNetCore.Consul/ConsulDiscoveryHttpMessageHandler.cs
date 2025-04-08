using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AspNetCore.Consul
{
    public class ConsulDiscoveryHttpMessageHandler : DelegatingHandler
    {
        ConsulServiceDiscoveryProviderAccessor _providerAccessor;
        ILogger _logger;
        ConsulDiscoveryHttpMessageHandlerOptions _options;

        public ConsulDiscoveryHttpMessageHandler(ConsulDiscoveryHttpMessageHandlerOptions options,
            ConsulServiceDiscoveryProviderAccessor providerAccessor,
            ILoggerFactory loggerFactory)
        {
            _providerAccessor = providerAccessor;
            _logger = loggerFactory.CreateLogger<ConsulDiscoveryHttpMessageHandler>();
            _options = options;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var current = request.RequestUri;
            try
            {
                var service = await LookupServiceAsync(current);
                if (service != null)
                {
                    var uri = new Uri($"{current.Scheme}://{service.Host}:{service.Port}");
                    request.RequestUri = new Uri(uri, current.PathAndQuery);
                    _logger.LogDebug("{0} has hanlded to {1}", current, request.RequestUri);
                }
                return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "GrpcDiscoveryHttpMessageHandler.SendAsync() occur errors");
                throw;
            }
            finally
            {
                request.RequestUri = current;
            }
        }
        /// <summary>
        /// 搜寻一个可用的服务
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private async Task<ConsulService> LookupServiceAsync(Uri uri)
        {
            if (!uri.IsDefaultPort)
            {
                return null;
            }
            var serviceName = uri.Host;

            ConsulServiceDiscoveryProviderOptions options;
            if (_options.Polling)
            {
                options = new PollConsulServiceDiscoveryProviderOptions()
                {
                    Interval = _options.Interval,
                    ServiceName = serviceName,
                    Mode = _options.Mode,
                    Tag = _options.Tag
                };
            }
            else
            {
                options = new ConsulServiceDiscoveryProviderOptions()
                {
                    ServiceName = serviceName,
                    Mode = _options.Mode,
                    Tag = _options.Tag
                };
            }

            var provider = _providerAccessor.GetProvider(_options.ConsulName, options);
            return await provider.GetService();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Issues: https://gitee.com/shanfeng1000/dotnetcore-demo/issues/I58VLH
            // _providerAccessor?.Dispose();
            // _providerAccessor = null;
        }
    }
    public class ConsulDiscoveryHttpMessageHandlerOptions
    {
        /// <summary>
        /// Consul节点名称
        /// </summary>
        public string ConsulName { get; set; }
        /// <summary>
        /// 是否使用Poll模式
        /// </summary>
        public bool Polling { get; set; }
        /// <summary>
        /// Poll的时间间隔（毫秒）
        /// </summary>
        public int Interval { get; set; }
        /// <summary>
        /// 服务标签
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 均衡模式
        /// </summary>
        public LoadBalancerMode Mode { get; set; } = LoadBalancerMode.Random;
    }
    public enum LoadBalancerMode
    {
        /// <summary>
        /// 随机
        /// </summary>
        Random = 0,
        /// <summary>
        /// 轮询
        /// </summary>
        RoundRobin = 1
    }
}
