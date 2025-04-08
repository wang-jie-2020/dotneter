using AspNetCore.Activemq.Integration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Activemq
{
    public interface IActiveConsumerBuilder
    {
        IServiceCollection Services { get; }

        IActiveConsumerBuilder AddListener(Action<IServiceProvider, RecieveResult> onMessageRecieved);
    }
}
