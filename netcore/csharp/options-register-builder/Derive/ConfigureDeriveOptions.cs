using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Demo.Derive
{
    public class ConfigureDeriveOptions : IConfigureOptions<DeriveOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConfigureDeriveOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(DeriveOptions options)
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
