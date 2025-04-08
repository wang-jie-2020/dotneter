using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul
{
    public class ConsulServiceBuilder : IConsulServiceBuilder
    {
        IServiceCollection services;

        public ConsulServiceBuilder(string name, IServiceCollection services)
        {
            Name = name;
            this.services = services;
        }

        public string Name { get; }

        public IConsulServiceBuilder AddService(ConsulServiceOptions consulServiceOptions)
        {
            services.Configure<ServiceRegistrationOptions>(Name, options =>
            {
                options.Services.Add(consulServiceOptions);
            });
            return this;
        }
    }
}
