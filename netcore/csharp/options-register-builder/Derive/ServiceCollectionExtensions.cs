using Demo.Core;
using System;

namespace Demo.Derive
{
    public static class ServiceCollectionExtensions
    {
        public static CoreOptions UseDerive(this CoreOptions options, Action<DeriveOptions>? configure = null)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new DeriveOptionsExtension(configure));

            return options;
        }
    }
}
