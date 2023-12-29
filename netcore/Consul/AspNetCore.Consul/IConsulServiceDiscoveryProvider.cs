using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Consul
{
    public interface IConsulServiceDiscoveryProvider : IDisposable
    {
        Task<List<ConsulService>> GetServices();
        Task<ConsulService> GetService();
    }
}
