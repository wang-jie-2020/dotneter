using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Demo.Blob.Storage.Impl
{
    public class BlobContainerFactory : IBlobContainerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BlobContainerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IBlobContainer Create(string name)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IOptions<BlobStorageOptions>>().Value
                    .Containers.GetConfiguration(name);

                return new BlobContainer(
                    name,
                    configuration,
                    (IBlobProvider)_serviceProvider.GetRequiredService(configuration.Provider),
                    _serviceProvider
                );
            }
        }
    }
}
