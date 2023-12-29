using Demo.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnitOfWorkContextRepository.Extensions;

namespace UnitOfWorkContextRepository.Test.Fixtures
{
    public class UowFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; }

        public UowFixture()
        {
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            //    .AddEnvironmentVariables()
            //    .Build();

            var services = new ServiceCollection();

            var connection = CreateMemoryConnection();
            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                builder.UseSqlite(connection);
            });

            services.AddUnitOfWorkRepository<ApplicationDbContext>();

            ServiceProvider = services.BuildServiceProvider();
        }

        private static SqliteConnection CreateMemoryConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options))
            {
                context.GetService<IRelationalDatabaseCreator>().EnsureCreated();
            }

            return connection;
        }

        public void Dispose()
        {

        }
    }
}
