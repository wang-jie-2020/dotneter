using Sms;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sms.ChinaMobile.Internal;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Sms.ChinaMobile
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
            services.TryAddEnumerable(ServiceDescriptor.Transient<ISmsService, ChinaMobileSmsService>());

            //Add ChinaMobileOptions
            services.Configure(_configure);
            services.AddSingleton<IConfigureOptions<ChinaMobileSmsOptions>, ConfigureChinaMobileSmsOptions>();
        }
    }
}
