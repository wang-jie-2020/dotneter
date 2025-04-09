using CheckCore;

namespace MinioStorage
{
    public class MinioProviderConfiguration
    {
        public string BucketName
        {
            get => _containerConfiguration.GetConfigurationOrDefault<string>(MinioProviderConfigurationNames.BucketName);
            set => _containerConfiguration.SetConfiguration(MinioProviderConfigurationNames.BucketName, value);
        }

        public string EndPoint
        {
            get => _containerConfiguration.GetConfigurationOrDefault<string>(MinioProviderConfigurationNames.EndPoint);
            set => _containerConfiguration.SetConfiguration(MinioProviderConfigurationNames.EndPoint, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public string AccessKey
        {
            get => _containerConfiguration.GetConfigurationOrDefault<string>(MinioProviderConfigurationNames.AccessKey);
            set => _containerConfiguration.SetConfiguration(MinioProviderConfigurationNames.AccessKey, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public string SecretKey
        {
            get => _containerConfiguration.GetConfigurationOrDefault<string>(MinioProviderConfigurationNames.SecretKey);
            set => _containerConfiguration.SetConfiguration(MinioProviderConfigurationNames.SecretKey, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }

        public bool WithSSL
        {
            get => _containerConfiguration.GetConfigurationOrDefault(MinioProviderConfigurationNames.WithSSL, false);
            set => _containerConfiguration.SetConfiguration(MinioProviderConfigurationNames.WithSSL, value);
        }

        /// <summary>
        ///     Default value: false.
        /// </summary>
        public bool CreateBucketIfNotExists
        {
            get => _containerConfiguration.GetConfigurationOrDefault(MinioProviderConfigurationNames.CreateBucketIfNotExists, false);
            set => _containerConfiguration.SetConfiguration(MinioProviderConfigurationNames.CreateBucketIfNotExists, value);
        }

        private readonly MinioContainerConfiguration _containerConfiguration;

        public MinioProviderConfiguration(MinioContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
}
