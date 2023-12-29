using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hugin.BookStore.EntityFrameworkCore
{
    public class
        BookStoreHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<
            BookStoreHostMigrationsDbContext>
    {
        public BookStoreHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<BookStoreHostMigrationsDbContext>().UseMySql(
                configuration.GetConnectionString(BookStoreConsts.DbProperties.ConnectionStringName),
                ServerVersion.AutoDetect(configuration.GetConnectionString(BookStoreConsts.DbProperties.ConnectionStringName)));

            return new BookStoreHostMigrationsDbContext(builder.Options);
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