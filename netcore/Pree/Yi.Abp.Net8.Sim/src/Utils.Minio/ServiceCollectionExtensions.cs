using System;
using Microsoft.Extensions.DependencyInjection;
using Utils.Minio.impl;

namespace Utils.Minio
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseMinioStorage(this IServiceCollection services,
            Action<MinioStorageOptions> configure = null)
        {
            var options = new MinioStorageOptions();
            configure?.Invoke(options);
            services.Configure(configure);

            services.AddTransient(typeof(IMinioContainer<>), typeof(MinioContainer<>));
            services.AddTransient<IMinioContainerFactory, MinioContainerFactory>();
            services.AddTransient<IMinioNameNormalizer, MinioNameNormalizer>();

            return services;
        }

        public static MinioContainerConfiguration UseMinio(this MinioContainerConfiguration containerConfiguration,
            Action<MinioProviderConfiguration> minioConfigureAction)
        {
            minioConfigureAction(new MinioProviderConfiguration(containerConfiguration));
            return containerConfiguration;
        }
    }
}
