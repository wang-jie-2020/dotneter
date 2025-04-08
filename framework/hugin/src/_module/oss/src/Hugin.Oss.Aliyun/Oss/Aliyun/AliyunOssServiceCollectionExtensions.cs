using System;

namespace Hugin.Oss.Aliyun
{
    public static class ServiceCollectionExtensions
    {
        public static OssOptions UseAliyun(this OssOptions options, Action<AliyunOssOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new AliyunOssOptionsExtension(configure));

            return options;
        }
    }
}
