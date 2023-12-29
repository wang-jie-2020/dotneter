using Consul;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.Consul
{
    public class PollConsulServiceDiscoveryProvider : ConsulServiceDiscoveryProvider
    {
        PollConsulServiceDiscoveryProviderOptions _options;
        private Timer _timer;
        private bool _polling;
        private List<ConsulService> _services;

        public PollConsulServiceDiscoveryProvider(PollConsulServiceDiscoveryProviderOptions options, IConsulClient consul, ILogger logger) : base(options, consul, logger)
        {
            _options = options;
            _services = new List<ConsulService>();

            _timer = new Timer(async _ =>
            {
                await Poll();
            }, null, options.Interval, options.Interval);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            _timer = null;

            base.Dispose();
        }

        public override async Task<List<ConsulService>> GetServices()
        {
            if (_services == null || _services.Count == 0)
            {
                await Poll();
            }

            ChechDisposed();
            return _services;
        }

        private async Task Poll()
        {
            if (_polling)
            {
                return;
            }

            _polling = true;
            _services = await base.GetServices();
            _polling = false;
        }
    }
    public class PollConsulServiceDiscoveryProviderOptions : ConsulServiceDiscoveryProviderOptions
    {
        /// <summary>
        /// Poll时间间隔（毫秒）
        /// </summary>
        public int Interval { get; set; }
    }
}
