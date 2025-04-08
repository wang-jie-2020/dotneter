using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hugin.Oss.Aliyun
{
    public class AliyunOssOptions
    {
        /// <summary>
        /// OSS的访问ID
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// OSS的访问密钥
        /// </summary>
        public string AccessKeySecret { get; set; }

        /// <summary>
        /// OSS的访问地址
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string BucketName { get; set; }
    }

    internal class ConfigureAliyunOssOptions : IConfigureOptions<AliyunOssOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConfigureAliyunOssOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(AliyunOssOptions options)
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
