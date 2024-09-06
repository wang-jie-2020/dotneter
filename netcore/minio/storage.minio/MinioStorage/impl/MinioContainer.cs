using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.Exceptions;

namespace MinioStorage.impl
{
    public class MinioContainer<TContainer> : IMinioContainer<TContainer>
        where TContainer : class
    {
        private readonly IMinioContainer _container;

        public MinioClient Client => _container.Client;

        public MinioContainer(IMinioContainerFactory minioContainerFactory)
        {
            _container = minioContainerFactory.Create(ContainerNameAttribute.GetContainerName<TContainer>());
        }

        public Task<List<string>> ListAsync(string prefix = "")
        {
            return _container.ListAsync(prefix);
        }

        public Task SaveAsync(string name, Stream stream, bool overrideExisting = false)
        {
            return _container.SaveAsync(name, stream, overrideExisting);
        }

        public Task<string> PublishAsync(string name, Stream stream, bool overrideExisting = false)
        {
            return _container.PublishAsync(name, stream, overrideExisting);
        }

        public Task<bool> ExistsAsync(string name)
        {
            return _container.ExistsAsync(name);
        }

        public Task<Stream> GetAsync(string name)
        {
            return _container.GetAsync(name);
        }

        public Task<bool> DeleteAsync(string name)
        {
            return _container.DeleteAsync(name);
        }

        public Task<string> PresignedGetAsync(string name, int expiry)
        {
            return _container.PresignedGetAsync(name, expiry);
        }

        public Task<string> PresignedSaveAsync(string name, int expiry, bool overrideExisting = false)
        {
            return _container.PresignedSaveAsync(name, expiry, overrideExisting);
        }
    }

    public class MinioContainer : IMinioContainer
    {
        private readonly string _containerName;
        private readonly MinioContainerConfiguration _configuration;
        private readonly MinioProviderConfiguration _providerConfiguration;

        private readonly MinioClient _client;
        private readonly IServiceProvider _serviceProvider;

        public MinioClient Client => _client;

        public MinioContainer(string containerName,
            MinioContainerConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _containerName = containerName;

            _configuration = configuration;
            _providerConfiguration = new MinioProviderConfiguration(configuration);

            _client = GetMinioClient(_providerConfiguration);

            _serviceProvider = serviceProvider;
        }

        protected MinioClient GetMinioClient(MinioProviderConfiguration providerConfiguration)
        {
            var client = new MinioClient()
                .WithEndpoint(providerConfiguration.EndPoint)
                .WithCredentials(providerConfiguration.AccessKey, providerConfiguration.SecretKey);

            if (providerConfiguration.WithSSL)
            {
                client.WithSSL();
            }

            return client.Build();
        }

        public async Task<List<string>> ListAsync(string prefix = "")
        {
            var bucketName = GetBucketName(_containerName);

            var result = new List<string>();

            if (await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName)))
            {
                var items = _client.ListObjectsAsync(new ListObjectsArgs()
                    .WithBucket(bucketName)
                    .WithPrefix(prefix)
                    .WithRecursive(true));
                items.Subscribe(
                    item => result.Add(item.Key),
                    ex => { },
                    () => { });

                await items;
            }

            return result;
        }

        public async Task SaveAsync(string name, Stream stream, bool overrideExisting = false)
        {
            var bucketName = GetBucketName(_containerName);
            var objectName = GetObjectName(name);

            if (!overrideExisting && await ExistsAsync(bucketName, objectName))
            {
                throw new Exception("already exists in the container");
            }

            if (_providerConfiguration.CreateBucketIfNotExists)
            {
                if (!await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName)))
                {
                    await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
                }
            }

            await _client.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length));
        }

        public async Task<string> PublishAsync(string name, Stream stream, bool overrideExisting = false)
        {
            await SaveAsync(name, stream, overrideExisting);

            var bucketName = GetBucketName(_containerName);
            var objectName = GetObjectName(name);
            return $"{(_providerConfiguration.WithSSL ? "https" : "http")}://{_providerConfiguration.EndPoint}/{bucketName}/{objectName}";
        }

        public async Task<bool> ExistsAsync(string name)
        {
            var bucketName = GetBucketName(_containerName);
            var objectName = GetObjectName(name);

            return await ExistsAsync(bucketName, objectName);
        }

        public async Task<Stream> GetAsync(string name)
        {
            var bucketName = GetBucketName(_containerName);
            var objectName = GetObjectName(name);

            if (!await ExistsAsync(bucketName, objectName))
            {
                return null;
            }

            var memoryStream = new MemoryStream();

            await _client.GetObjectAsync(new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithCallbackStream(stream =>
                {
                    if (stream != null)
                    {
                        stream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                    else
                    {
                        memoryStream = null;
                    }
                }));

            return memoryStream;
        }

        public async Task<bool> DeleteAsync(string name)
        {
            var bucketName = GetBucketName(_containerName);
            var objectName = GetObjectName(name);

            if (await ExistsAsync(bucketName, objectName))
            {
                await _client.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName));

                return true;
            }

            return false;
        }

        public async Task<string> PresignedGetAsync(string name, int expiry)
        {
            var bucketName = GetBucketName(_containerName);
            var objectName = GetObjectName(name);

            if (await ExistsAsync(bucketName, objectName))
            {
                return await _client.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                     .WithBucket(bucketName)
                     .WithObject(objectName)
                     .WithExpiry(expiry));
            }

            return string.Empty;
        }

        public async Task<string> PresignedSaveAsync(string name, int expiry, bool overrideExisting = false)
        {
            var bucketName = GetBucketName(_containerName);
            var objectName = GetObjectName(name);

            if (!overrideExisting && await ExistsAsync(bucketName, objectName))
            {
                throw new Exception("already exists in the container");
            }

            if (_providerConfiguration.CreateBucketIfNotExists)
            {
                if (!await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName)))
                {
                    await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
                }
            }

            return await _client.PresignedPutObjectAsync(new PresignedPutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithExpiry(expiry));
        }

        protected async Task<bool> ExistsAsync(string bucketName, string objectName)
        {
            if (await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName)))
            {
                try
                {
                    await _client.StatObjectAsync(new StatObjectArgs().WithBucket(bucketName).WithObject(objectName));
                }
                catch (Exception e)
                {
                    if (e is ObjectNotFoundException)
                    {
                        return false;
                    }

                    throw;
                }

                return true;
            }

            return false;
        }

        protected virtual string GetBucketName(string name)
        {
            //如果配置中指定存储桶就忽略强类型约定
            if (!string.IsNullOrWhiteSpace(_providerConfiguration.BucketName))
            {
                return _providerConfiguration.BucketName;
            }

            //如果配置中未指定,就按照类型约定
            using (var scope = _serviceProvider.CreateScope())
            {
                //foreach (var normalizeNamingServiceType in _configuration.NormalizeNamingServiceTypes)
                //{
                //    var normalizeNamingService = (INormalizeNamingService)scope.ServiceProvider
                //        .GetRequiredService(normalizeNamingServiceType);

                //    name = normalizeNamingService.NormalizeContainerNaming(_configuration, name);
                //}

                var normalizer = scope.ServiceProvider.GetRequiredService<IMinioNameNormalizer>();
                return normalizer.NormalizeBucketName(name);
            }
        }

        protected virtual string GetObjectName(string name)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                //foreach (var normalizeNamingServiceType in _configuration.NormalizeNamingServiceTypes)
                //{
                //    var normalizeNamingService = (INormalizeNamingService)scope.ServiceProvider
                //        .GetRequiredService(normalizeNamingServiceType);

                //    name = normalizeNamingService.NormalizeObjectNaming(_configuration, name);
                //}

                var normalizer = scope.ServiceProvider.GetRequiredService<IMinioNameNormalizer>();
                return normalizer.NormalizeObjectName(name);
            }
        }
    }
}
