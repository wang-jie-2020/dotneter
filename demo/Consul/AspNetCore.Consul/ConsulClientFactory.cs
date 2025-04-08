using Consul;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul
{
    public class ConsulClientFactory : IConsulClientFactory
    {
        private readonly IOptionsMonitor<ConsulClientOptions> _optionsMonitor;

        public ConsulClientFactory(IOptionsMonitor<ConsulClientOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        /// <summary>
        /// 创建一个Consul客户端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IConsulClient CreateClient(string name)
        {
            var options = _optionsMonitor.Get(name);
            return CreateClient(options);
        }
        /// <summary>
        /// 创建一个Consul客户端
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public IConsulClient CreateClient(ConsulClientOptions options)
        {
            if (string.IsNullOrEmpty(options.Address))
            {
                throw new ArgumentException($"{nameof(ConsulClientOptions.Address)} can not be empty", nameof(ConsulClientOptions.Address));
            }

            return new ConsulClient(config =>
            {
                config.Address = new Uri(options.Address);
                if (!string.IsNullOrEmpty(options.Datacenter))
                {
                    config.Datacenter = options.Datacenter;
                }
                if (!string.IsNullOrEmpty(options.Token))
                {
                    config.Token = options.Token;
                }
            });
        }
    }
}
