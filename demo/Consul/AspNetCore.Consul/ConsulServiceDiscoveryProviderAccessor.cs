using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace AspNetCore.Consul
{
    public class ConsulServiceDiscoveryProviderAccessor : IDisposable
    {
        ConcurrentDictionary<DiscoveryProviderKey, IConsulServiceDiscoveryProvider> providers = new ConcurrentDictionary<DiscoveryProviderKey, IConsulServiceDiscoveryProvider>();

        IConsulClientFactory _consulClientFactory;
        ILoggerFactory _loggerFactory;
        bool disposed = false;

        public ConsulServiceDiscoveryProviderAccessor(IConsulClientFactory consulClientFactory, ILoggerFactory loggerFactory)
        {
            _consulClientFactory = consulClientFactory;
            _loggerFactory = loggerFactory;
        }

        public void Dispose()
        {
            foreach (var key in providers.Keys)
            {
                if (providers.TryGetValue(key, out IConsulServiceDiscoveryProvider provider))
                {
                    provider.Dispose();
                }
            }
            disposed = true;
        }

        public IConsulServiceDiscoveryProvider GetProvider(string consulName, ConsulServiceDiscoveryProviderOptions options)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(ConsulServiceDiscoveryProviderAccessor));
            }

            var key = new DiscoveryProviderKey(consulName, options.ServiceName);
            string name = options.ServiceName;
            return providers.GetOrAdd(key, _ =>
            {
                var client = _consulClientFactory.CreateClient(consulName);
                if (options is PollConsulServiceDiscoveryProviderOptions pollOptions)
                {
                    return new PollConsulServiceDiscoveryProvider(pollOptions, client, _loggerFactory.CreateLogger<PollConsulServiceDiscoveryProvider>());
                }
                return new ConsulServiceDiscoveryProvider(options, client, _loggerFactory.CreateLogger<ConsulServiceDiscoveryProvider>());
            });
        }
        public class DiscoveryProviderKey : IEquatable<DiscoveryProviderKey>
        {
            public DiscoveryProviderKey(string consulName, string serviceName) => (ConsulName, ServiceName) = (consulName, serviceName);

            public string ConsulName { get; private set; }
            public string ServiceName { get; private set; }

            public bool Equals([AllowNull] DiscoveryProviderKey other)
            {
                return other != null && ConsulName == other.ConsulName && string.Equals(ServiceName, other.ServiceName, StringComparison.OrdinalIgnoreCase);
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as DiscoveryProviderKey);
            }
            public override int GetHashCode()
            {
                return ConsulName.GetHashCode() * 11 + ServiceName.GetHashCode();
            }
        }
    }
}
