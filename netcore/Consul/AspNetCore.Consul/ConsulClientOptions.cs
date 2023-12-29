using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul
{
    public class ConsulClientOptions
    {
        /// <summary>
        /// Consul注册中心地址（http://host:port）
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 数据中心
        /// </summary>
        public string Datacenter { get; set; } = "dc1";
        /// <summary>
        /// API token
        /// </summary>
        public string Token { get; set; }
    }
}
