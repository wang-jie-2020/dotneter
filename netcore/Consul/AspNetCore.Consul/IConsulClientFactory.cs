using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul
{
    public interface IConsulClientFactory
    {
        /// <summary>
        /// 创建一个Consul客户端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IConsulClient CreateClient(string name);
    }
}
