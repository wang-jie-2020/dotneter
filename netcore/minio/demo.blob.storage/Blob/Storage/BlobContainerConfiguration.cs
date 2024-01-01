using System;
using System.Collections.Generic;
using Demo.Blob.Storage.Utils;
using JetBrains.Annotations;

namespace Demo.Blob.Storage
{
    public class BlobContainerConfiguration
    {
        private readonly BlobContainerConfiguration _fallbackConfiguration;

        public Type Provider { get; set; }

        /// <summary>
        ///     也许是一个list?
        /// </summary>
        public Type NamingNormalizer { get; set; }

        /// <summary>
        ///     配置:不同的Provider不同的配置对象以字典存储
        /// </summary>
        [NotNull]
        private readonly Dictionary<string, object> _properties;

        public BlobContainerConfiguration(BlobContainerConfiguration fallbackConfiguration = null)
        {
            _fallbackConfiguration = fallbackConfiguration;
            _properties = new Dictionary<string, object>();
        }

        [CanBeNull]
        public T GetConfigurationOrDefault<T>(string name, T defaultValue = default)
        {
            return (T)GetConfigurationOrNull(name, defaultValue);
        }

        [CanBeNull]
        public object GetConfigurationOrNull(string name, object defaultValue = null)
        {
            return _properties.TryGetValue(name, out var obj)
                ? obj
                : _fallbackConfiguration?.GetConfigurationOrNull(name, defaultValue) ?? defaultValue;
        }

        [NotNull]
        public BlobContainerConfiguration SetConfiguration([NotNull] string name, [CanBeNull] object value)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(value, nameof(value));
            _properties[name] = value;

            return this;
        }

        [NotNull]
        public BlobContainerConfiguration RemoveConfiguration([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            _properties.Remove(name);

            return this;
        }
    }
}
