using Hugin.Sms.Aliyun;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Hugin.Sms
{
    public class SampleTest
    {
        [Fact]
        public void ShouldEquals()
        {
            var services = new ServiceCollection();

            services.AddSms(x =>
            {
                x.Version = "v1.0";
                x.UseAliyun(z =>
                {
                    z.AccessKeyId = "";
                    z.AccessKeySecret = "";
                });
            });

            var sp = services.BuildServiceProvider();

            var smsOptions = sp.GetRequiredService<IOptions<SmsOptions>>();
            var smsService = sp.GetRequiredService<ISmsService>();

            Assert.Equal("v1.0", smsOptions.Value.Version);
            Assert.NotNull(smsService);
        }
    }
}
