using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sms.ChinaMobile;
using Xunit;

namespace Sms.Test
{
    public class ServicesTest
    {
        [Fact]
        public void ShouldInjected()
        {
            var services = new ServiceCollection();

            services.AddSms(x =>
            {
                x.Version = "v1.0";
                x.UseChinaMobile(z =>
                {
                    z.AppId = "test";
                    z.AppSecret = "test";
                });
            });

            var sp = services.BuildServiceProvider();

            var smsOptions = sp.GetRequiredService<IOptions<SmsOptions>>();
            var chinaMobileOptions = sp.GetRequiredService<IOptions<ChinaMobileSmsOptions>>();
            var smsService = sp.GetRequiredService<ISmsService>();

            Assert.Equal("v1.0", smsOptions.Value.Version);
            Assert.Equal("test", chinaMobileOptions.Value.AppId);
            Assert.NotNull(smsService);
        }
    }
}
