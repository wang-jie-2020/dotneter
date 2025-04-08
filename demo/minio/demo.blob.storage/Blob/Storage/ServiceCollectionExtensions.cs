using System;
using Demo.Blob.Storage.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Blob.Storage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlobStorage(this IServiceCollection services, Action<BlobStorageOptions> configure = null)
        {
            var options = new BlobStorageOptions(services);
            configure?.Invoke(options);

            services.AddTransient<IBlobContainerFactory, BlobContainerFactory>();
            services.AddTransient(typeof(IBlobContainer<>), typeof(BlobContainer<>));

            services.Configure(configure);

            return services;
        }
    }
}