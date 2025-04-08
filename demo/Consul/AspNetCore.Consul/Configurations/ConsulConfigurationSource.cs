using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul.Configurations
{
    public class ConsulConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// 源状态信息
        /// </summary>
        public ConsulConfigurationOptions ConsulConfigurationOptions { get; private set; }

        public ConsulConfigurationSource(ConsulConfigurationOptions consulConfigurationOptions)
        {
            this.ConsulConfigurationOptions = consulConfigurationOptions;
        }

        /// <summary>
        /// 获取配置提供者
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConsulConfigurationProvider(this);
        }
    }
}
