using System;
using System.IO;
using System.Threading.Tasks;
using Demo.Blob.Storage.ViewModels;
using Minio;
using Minio.Exceptions;

namespace Demo.Blob.Storage.Minio
{
    public class MinioBlobProvider : IBlobProvider
    {
        public async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var configuration = new MinioBlobProviderConfiguration(args.Configuration);
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);
            var blobName = GetBlobName(args);

            if (!args.OverrideExisting && await BlobExistsAsync(client, containerName, blobName))
            {
                throw new Exception("already exists in the container");
            }

            if (configuration.CreateBucketIfNotExists)
            {
                await CreateBucketIfNotExists(client, containerName);
            }

            await client.PutObjectAsync(new PutObjectArgs()
                .WithBucket(containerName)
                .WithObject(blobName)
                .WithStreamData(args.BlobStream)
                .WithObjectSize(args.BlobStream.Length));
        }

        public async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);
            var blobName = GetBlobName(args);

            if (await BlobExistsAsync(client, containerName, blobName))
            {
                await client.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(containerName)
                    .WithObject(blobName));

                return true;
            }

            return false;
        }

        public async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);
            var blobName = GetBlobName(args);

            return await BlobExistsAsync(client, containerName, blobName);
        }

        public async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);
            var blobName = GetBlobName(args);

            if (!await BlobExistsAsync(client, containerName, blobName))
            {
                return null;
            }

            var memoryStream = new MemoryStream();

            await client.GetObjectAsync(new GetObjectArgs()
                .WithBucket(containerName)
                .WithObject(blobName)
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

        protected virtual MinioClient GetMinioClient(BlobProviderArgs args)
        {
            var configuration = new MinioBlobProviderConfiguration(args.Configuration);

            var client = new MinioClient()
                .WithEndpoint(configuration.EndPoint)
                .WithCredentials(configuration.AccessKey, configuration.SecretKey);

            if (configuration.WithSSL)
            {
                client.WithSSL();
            }

            return client.Build();
        }

        protected virtual async Task CreateBucketIfNotExists(MinioClient client, string containerName)
        {
            if (!await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(containerName)))
            {
                await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(containerName));
            }
        }

        protected virtual async Task<bool> BlobExistsAsync(MinioClient client, string containerName, string blobName)
        {
            if (await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(containerName)))
            {
                try
                {
                    await client.StatObjectAsync(new StatObjectArgs().WithBucket(containerName).WithObject(blobName));
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

        protected virtual string GetContainerName(BlobProviderArgs args)
        {
            var configuration = new MinioBlobProviderConfiguration(args.Configuration);
            return configuration.BucketName ?? args.ContainerName;
        }

        protected virtual string GetBlobName(BlobProviderArgs args)
        {
            return args.BlobName;
        }
    }
}
