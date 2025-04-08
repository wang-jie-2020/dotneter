using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Hugin.Oss.Aliyun
{
    public class AliyunOssOptionsExtension : IOssOptionsExtension
    {
        private readonly Action<AliyunOssOptions> _configure;

        public AliyunOssOptionsExtension(Action<AliyunOssOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IOssService, AliyunOssService>());

            services.Configure(_configure);
            services.AddSingleton<IConfigureOptions<AliyunOssOptions>, ConfigureAliyunOssOptions>();
        }
    }
}
