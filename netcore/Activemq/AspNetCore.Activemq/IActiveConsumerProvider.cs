using AspNetCore.Activemq.Integration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Activemq
{
    public interface IActiveConsumerProvider : IDisposable
    {
        ActiveConsumer Consumer { get; }

        Task ListenAsync();
    }
}
