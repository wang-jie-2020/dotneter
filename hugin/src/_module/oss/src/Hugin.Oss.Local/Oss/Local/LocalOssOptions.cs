using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hugin.Oss.Local
{
    public class LocalOssOptions
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string Endpoint { get; set; }
    }

    internal class ConfigureLocalOssOptions : IConfigureOptions<LocalOssOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConfigureLocalOssOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(LocalOssOptions options)
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
