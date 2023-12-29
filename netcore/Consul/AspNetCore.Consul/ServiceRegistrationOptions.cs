using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul
{
    public class ServiceRegistrationOptions
    {
        /// <summary>
        /// 服务列表
        /// </summary>
        public ICollection<ConsulServiceOptions> Services { get; } = new List<ConsulServiceOptions>();
    }
}
