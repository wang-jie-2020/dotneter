using System;
using Microsoft.Extensions.Options;

namespace MinioStorage.impl
{
    public class MinioContainerFactory : IMinioContainerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MinioContainerConfigurations _configurations;

        public MinioContainerFactory(IOptions<MinioStorageOptions> options,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _configurations = options.Value.Containers;
        }

        public IMinioContainer Create(string name)
        {
            var configuration = _configurations.GetConfiguration(name);
            return new MinioContainer(name, configuration, _serviceProvider);
        }
    }
}
