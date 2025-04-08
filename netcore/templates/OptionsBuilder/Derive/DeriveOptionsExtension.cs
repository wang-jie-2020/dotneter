using Demo.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Demo.Derive
{
    public class DeriveOptionsExtension : ICoreOptionsExtension
    {
        private readonly Action<DeriveOptions>? _configure;

        public DeriveOptionsExtension(Action<DeriveOptions>? configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.Configure<DeriveOptions>(_configure);

            services.AddSingleton<IConfigureOptions<DeriveOptions>, ConfigureDeriveOptions>();
        }
    }
}
