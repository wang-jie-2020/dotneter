using System;

namespace Demo.Blob.Storage.Minio
{
    public static class MinioBlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseMinio(
            this BlobContainerConfiguration containerConfiguration,
            Action<MinioBlobProviderConfiguration> minioConfigureAction)
        {
            containerConfiguration.Provider = typeof(MinioBlobProvider);
            containerConfiguration.NamingNormalizer = typeof(MinioBlobNamingNormalizer);

            minioConfigureAction(new MinioBlobProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }
    }
}
