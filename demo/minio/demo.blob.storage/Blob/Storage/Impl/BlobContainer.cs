using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Demo.Blob.Storage.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Blob.Storage.Impl
{
    public class BlobContainer<TContainer> : IBlobContainer<TContainer>
        where TContainer : class
    {
        private readonly IBlobContainer _container;

        public BlobContainer(IBlobContainerFactory blobContainerFactory)
        {
            _container = blobContainerFactory.Create(BlobContainerNameAttribute.GetContainerName<TContainer>());
        }

        public Task SaveAsync(
            string name,
            Stream stream,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default)
        {
            return _container.SaveAsync(
                name,
                stream,
                overrideExisting,
                cancellationToken
            );
        }

        public Task<bool> DeleteAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.DeleteAsync(
                name,
                cancellationToken
            );
        }

        public Task<bool> ExistsAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.ExistsAsync(
                name,
                cancellationToken
            );
        }

        public Task<Stream> GetAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.GetAsync(
                name,
                cancellationToken
            );
        }

        public Task<Stream> GetOrNullAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.GetOrNullAsync(
                name,
                cancellationToken
            );
        }
    }

    public class BlobContainer : IBlobContainer
    {
        private readonly string _containerName;
        private readonly BlobContainerConfiguration _configuration;
        private readonly IBlobProvider _provider;
        private readonly IServiceProvider _serviceProvider;

        public BlobContainer(
            string containerName,
            BlobContainerConfiguration configuration,
            IBlobProvider provider,
            IServiceProvider serviceProvider)
        {
            _containerName = containerName;
            _configuration = configuration;
            _provider = provider;
            _serviceProvider = serviceProvider;
        }

        public async Task SaveAsync(string name, Stream stream, bool overrideExisting = false, CancellationToken cancellationToken = default)
        {
            var blobNormalizeNaming = NormalizeNaming(_configuration, _containerName, name);

            await _provider.SaveAsync(
                new BlobProviderSaveArgs(
                    blobNormalizeNaming.Item1,
                    _configuration,
                    blobNormalizeNaming.Item2,
                    stream,
                    overrideExisting,
                    cancellationToken
                )
            );
        }

        public async Task<bool> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            var blobNormalizeNaming = NormalizeNaming(_configuration, _containerName, name);

            return await _provider.DeleteAsync(
                new BlobProviderDeleteArgs(
                    blobNormalizeNaming.Item1,
                    _configuration,
                    blobNormalizeNaming.Item2,
                    cancellationToken
                )
            );
        }

        public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            var blobNormalizeNaming = NormalizeNaming(_configuration, _containerName, name);

            return await _provider.ExistsAsync(
                new BlobProviderExistsArgs(
                    blobNormalizeNaming.Item1,
                    _configuration,
                    blobNormalizeNaming.Item2,
                    cancellationToken
                )
            );
        }

        public async Task<Stream> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            var stream = await GetOrNullAsync(name, cancellationToken);

            return stream;
        }

        public async Task<Stream> GetOrNullAsync(string name, CancellationToken cancellationToken = default)
        {
            var blobNormalizeNaming = NormalizeNaming(_configuration, _containerName, name);

            return await _provider.GetOrNullAsync(
                new BlobProviderGetArgs(
                    blobNormalizeNaming.Item1,
                    _configuration,
                    blobNormalizeNaming.Item2,
                    cancellationToken
                )
            );
        }

        protected virtual (string, string) NormalizeNaming(BlobContainerConfiguration configuration, string containerName, string blobName)
        {
            if (configuration.NamingNormalizer == null)
            {
                return (containerName, blobName);
            }

            var normalizer = (IBlobNamingNormalizer)_serviceProvider.GetRequiredService(configuration.NamingNormalizer);
            return (normalizer.NormalizeContainerName(containerName), normalizer.NormalizeBlobName(blobName));
        }
    }
}

