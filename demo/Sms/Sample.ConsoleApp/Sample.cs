using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Sms;

namespace Sample.ConsoleApp
{
    public class Sample
    {
        public static void ServiceSample()
        {
            var services = new ServiceCollection();

            services.AddSms(x =>
            {
                x.UseAliyun(z =>
                {
                    z.AccessKeyId = "AccessKey";
                    z.AccessKeySecret = "AccessKeySecret";
                    z.Sign = "Sign";
                });
            });

            var sp = services.BuildServiceProvider();
            var smsService = sp.GetRequiredService<ISmsService>();

            smsService.SendTemplateSms(new[] { "1234567890" }, "SMS001", new Dictionary<string, string>()
            {
                {"code", "123456"}
            });
        }

    }
}
