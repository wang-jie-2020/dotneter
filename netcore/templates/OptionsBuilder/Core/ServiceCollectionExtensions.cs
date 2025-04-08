using Microsoft.Extensions.DependencyInjection;
using System;

namespace Demo.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, Action<CoreOptions>? configure = null)
        {
            var options = new CoreOptions();
            configure?.Invoke(options);

            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }

            services.Configure(configure);

            return services;
        }
    }
}
