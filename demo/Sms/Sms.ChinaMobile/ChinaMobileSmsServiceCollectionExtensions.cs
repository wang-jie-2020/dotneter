using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sms.ChinaMobile;
using Sms;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static SmsOptions UseChinaMobile(this SmsOptions options, Action<ChinaMobileSmsOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new ChinaMobileSmsOptionsExtension(configure));

            return options;
        }
    }
}
