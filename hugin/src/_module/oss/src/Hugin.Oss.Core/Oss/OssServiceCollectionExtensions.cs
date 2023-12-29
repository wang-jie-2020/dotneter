using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Hugin.Oss
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ServiceCollection;

        public static IServiceCollection AddOss(this IServiceCollection services, Action<OssOptions> setupAction = null)
        {
            if (services.Any(x => x.ServiceType == typeof(OssOptions)))
            {
                throw new InvalidOperationException("oss services already registered");
            }

            ServiceCollection = services;

            //Inject Services

            //Options and extension service
            var options = new OssOptions();
            setupAction?.Invoke(options);

            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }
            services.Configure(setupAction);

            return services;
        }
    }
}
