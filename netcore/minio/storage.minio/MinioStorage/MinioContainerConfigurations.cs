using CheckCore;
using System;
using System.Collections.Generic;

namespace MinioStorage
{
    public class MinioContainerConfigurations
    {
        /// <summary>
        ///     全局默认配置
        /// </summary>
        private MinioContainerConfiguration Default => GetConfiguration("default");

        private readonly Dictionary<string, MinioContainerConfiguration> _containers;

        public MinioContainerConfigurations()
        {
            _containers = new Dictionary<string, MinioContainerConfiguration>()
            {
                ["default"] = new MinioContainerConfiguration()
            };
        }

        public MinioContainerConfigurations Configure<TContainer>(Action<MinioContainerConfiguration> configureAction)
        {
            return Configure(ContainerNameAttribute.GetContainerName<TContainer>(), configureAction);
        }

        public MinioContainerConfigurations Configure(string name, Action<MinioContainerConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));

            if (!_containers.TryGetValue(name, out var configuration))
            {
                configuration = new MinioContainerConfiguration(Default);
                _containers.Add(name, configuration);
            }

            configureAction(configuration);

            return this;
        }

        public MinioContainerConfigurations ConfigureAll(Action<string, MinioContainerConfiguration> configureAction)
        {
            foreach (var container in _containers)
            {
                configureAction(container.Key, container.Value);
            }

            return this;
        }

        public MinioContainerConfiguration GetConfiguration<TContainer>()
        {
            return GetConfiguration(ContainerNameAttribute.GetContainerName<TContainer>());
        }

        public MinioContainerConfiguration GetConfiguration(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _containers.TryGetValue(name, out var configuration) ? configuration : Default;
        }
    }
}
