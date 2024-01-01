using System;
using System.Collections.Generic;
using Demo.Blob.Storage.Utils;
using JetBrains.Annotations;

namespace Demo.Blob.Storage
{
    public class BlobContainerConfigurations
    {
        private BlobContainerConfiguration Default => GetConfiguration("default");

        private readonly Dictionary<string, BlobContainerConfiguration> _containers;

        public BlobContainerConfigurations()
        {
            _containers = new Dictionary<string, BlobContainerConfiguration>()
            {
                ["default"] = new BlobContainerConfiguration()
            };
        }

        public BlobContainerConfigurations Configure<TContainer>(
            Action<BlobContainerConfiguration> configureAction)
        {
            return Configure(
                BlobContainerNameAttribute.GetContainerName<TContainer>(),
                configureAction
            );
        }

        public BlobContainerConfigurations Configure(
            [NotNull] string name,
            [NotNull] Action<BlobContainerConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));

            if (!_containers.TryGetValue(name, out var value))
            {
                value = new BlobContainerConfiguration(Default);
                _containers.Add(name, value);
            }

            configureAction(value);

            return this;
        }

        public BlobContainerConfigurations ConfigureAll(Action<string, BlobContainerConfiguration> configureAction)
        {
            foreach (var container in _containers)
            {
                configureAction(container.Key, container.Value);
            }

            return this;
        }

        [NotNull]
        public BlobContainerConfiguration GetConfiguration<TContainer>()
        {
            return GetConfiguration(BlobContainerNameAttribute.GetContainerName<TContainer>());
        }

        [NotNull]
        public BlobContainerConfiguration GetConfiguration([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            return _containers.TryGetValue(name, out var value) ? value : Default;
        }
    }
}
