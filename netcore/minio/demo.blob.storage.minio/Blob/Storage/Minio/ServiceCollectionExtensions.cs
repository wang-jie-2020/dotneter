using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo.Blob.Storage.Minio
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMinioStorage(this IServiceCollection services)
        {
            services.AddTransient<MinioBlobNamingNormalizer>();
            services.AddTransient<MinioBlobProvider>();

            services.TryAddEnumerable(ServiceDescriptor.Transient<IBlobNamingNormalizer, MinioBlobNamingNormalizer>());
            services.TryAddEnumerable(ServiceDescriptor.Transient<IBlobProvider, MinioBlobProvider>());
        }
    }
}
