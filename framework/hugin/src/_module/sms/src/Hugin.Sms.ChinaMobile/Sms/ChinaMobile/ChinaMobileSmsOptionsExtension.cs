using System;
using Hugin.Sms.ChinaMobile.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Hugin.Sms.ChinaMobile
{
    public class ChinaMobileSmsOptionsExtension : ISmsOptionsExtension
    {
        private readonly Action<ChinaMobileSmsOptions> _configure;

        public ChinaMobileSmsOptionsExtension(Action<ChinaMobileSmsOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            //Inject Services
            services.TryAddTransient<ChinaMobileHttpClient>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<ISmsService, ChinaMobileService>());

            //Add ChinaMobileOptions
            services.Configure(_configure);
            services.AddSingleton<IConfigureOptions<ChinaMobileSmsOptions>, ConfigureChinaMobileSmsOptions>();
        }
    }
}
