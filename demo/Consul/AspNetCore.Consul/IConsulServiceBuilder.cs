using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul
{
    public interface IConsulServiceBuilder
    {
        public string Name { get; }
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="consulServiceOptions"></param>
        /// <returns></returns>
        IConsulServiceBuilder AddService(ConsulServiceOptions consulServiceOptions);
    }
}
