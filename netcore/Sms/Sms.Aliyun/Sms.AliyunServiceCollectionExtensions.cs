using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sms.ChinaMobile;
using Sms;
using Sms.Aliyun;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static SmsOptions UseAliyun(this SmsOptions options, Action<AliyunSmsOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new AliyunOptionsExtension(configure));

            return options;
        }
    }
}
