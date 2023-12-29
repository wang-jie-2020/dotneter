using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sms;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ServiceCollection;

        public static IServiceCollection AddSms(this IServiceCollection services, Action<SmsOptions> setupAction = null)
        {
            if (services.Any(x => x.ServiceType == typeof(SmsOptions)))
                throw new InvalidOperationException("SMS services already registered");

            ServiceCollection = services;

            //Inject Services

            //Options and extension service
            var options = new SmsOptions();
            setupAction?.Invoke(options);

            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }
            services.Configure<SmsOptions>(setupAction);

            return services;
        }
    }
}
