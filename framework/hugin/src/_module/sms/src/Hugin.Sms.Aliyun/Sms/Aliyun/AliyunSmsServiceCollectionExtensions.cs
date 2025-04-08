using System;

namespace Hugin.Sms.Aliyun
{
    public static class ServiceCollectionExtensions
    {
        public static SmsOptions UseAliyun(this SmsOptions options, Action<AliyunSmsOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new AliyunSmsOptionsExtension(configure));

            return options;
        }
    }
}
