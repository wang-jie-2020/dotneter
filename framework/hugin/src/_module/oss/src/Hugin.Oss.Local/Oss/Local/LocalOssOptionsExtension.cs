using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Hugin.Oss.Local
{
    public class LocalOssOptionsExtension : IOssOptionsExtension
    {
        private readonly Action<LocalOssOptions> _configure;

        public LocalOssOptionsExtension(Action<LocalOssOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            //Inject Services
            services.TryAddEnumerable(ServiceDescriptor.Transient<IOssService, LocalOssService>());

            services.Configure(_configure);
            services.AddSingleton<IConfigureOptions<LocalOssOptions>, ConfigureLocalOssOptions>();

        }
    }
}
