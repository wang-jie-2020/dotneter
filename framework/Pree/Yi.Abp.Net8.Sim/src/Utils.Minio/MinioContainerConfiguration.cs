using System.Collections.Generic;

namespace Utils.Minio
{
    public class MinioContainerConfiguration
    {
        private readonly MinioContainerConfiguration? _fallbackConfiguration;

        private readonly Dictionary<string, object> _properties;

        public MinioContainerConfiguration(MinioContainerConfiguration? fallbackConfiguration = null)
        {
            _fallbackConfiguration = fallbackConfiguration;
            _properties = new Dictionary<string, object>();
        }

        public T GetConfigurationOrDefault<T>(string name, T defaultValue = default)
        {
            return (T)GetConfigurationOrNull(name, defaultValue);
        }

        public object GetConfigurationOrNull(string name, object defaultValue = null)
        {
            return _properties.TryGetValue(name, out var obj)
                ? obj
                : _fallbackConfiguration?.GetConfigurationOrNull(name, defaultValue) ?? defaultValue;
        }

        public MinioContainerConfiguration SetConfiguration(string name, object value)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(value, nameof(value));

            _properties[name] = value;
            return this;
        }

        public MinioContainerConfiguration RemoveConfiguration(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            _properties.Remove(name);
            return this;
        }
    }
}
