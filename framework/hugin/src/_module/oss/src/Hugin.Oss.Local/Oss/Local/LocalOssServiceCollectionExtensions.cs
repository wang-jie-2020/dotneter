using System;

namespace Hugin.Oss.Local
{
    public static class ServiceCollectionExtensions
    {
        public static OssOptions UseLocal(this OssOptions options, Action<LocalOssOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new LocalOssOptionsExtension(configure));

            return options;
        }
    }
}
