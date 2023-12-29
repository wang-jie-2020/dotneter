using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Sms.ChinaMobile
{
    public class ChinaMobileSmsOptions
    {
        public string EcName { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string Sign { get; set; }

        public string AddSerial { get; set; }

        public string NorSubmitUrl { get; set; }

        public string TmpSubmitUrl { get; set; }

    }

    internal class ConfigureChinaMobileSmsOptions : IConfigureOptions<ChinaMobileSmsOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ConfigureChinaMobileSmsOptions(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(ChinaMobileSmsOptions options)
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
