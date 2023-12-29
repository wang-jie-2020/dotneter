using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Hugin.Sms.Aliyun
{
    public class AliyunSmsOptionsExtension : ISmsOptionsExtension
    {
        private readonly Action<AliyunSmsOptions> _configure;

        public AliyunSmsOptionsExtension(Action<AliyunSmsOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            //Inject Services
            services.TryAddEnumerable(ServiceDescriptor.Transient<ISmsService, AliyunSmsService>());
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IAliSmsService, AliyunSmsService>());

            services.Configure(_configure);
            services.AddSingleton<IConfigureOptions<AliyunSmsOptions>, ConfigureAliyunSmsOptions>();
        }
    }
}
