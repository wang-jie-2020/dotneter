using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hugin.Platform.EntityFrameworkCore
{
    public class PlatformHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<PlatformHostMigrationsDbContext>
    {
        public PlatformHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<PlatformHostMigrationsDbContext>().UseMySql(
                configuration.GetConnectionString(PlatformConsts.DbProperties.ConnectionStringName),
                ServerVersion.AutoDetect(configuration.GetConnectionString(PlatformConsts.DbProperties.ConnectionStringName)));

            return new PlatformHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false);

            return builder.Build();
        }
    }
}