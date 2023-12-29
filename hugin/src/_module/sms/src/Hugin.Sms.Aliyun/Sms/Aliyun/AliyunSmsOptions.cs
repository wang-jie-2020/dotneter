using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hugin.Sms.Aliyun
{
    public class AliyunSmsOptions
    {
        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        public string Sign { get; set; }
    }

    internal class ConfigureAliyunSmsOptions : IConfigureOptions<AliyunSmsOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConfigureAliyunSmsOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(AliyunSmsOptions options)
        {
            /*
             *  Do Extra Configure
             *  example:
             *
                    if (options.DbContextType != null)
                    {
                        using var scope = _serviceScopeFactory.CreateScope();
                        var provider = scope.ServiceProvider;
                        using var dbContext = (DbContext)provider.GetRequiredService(options.DbContextType);
                        options.ConnectionString = dbContext.Database.GetDbConnection().ConnectionString;
                    }
             *
             */
        }
    }
}
