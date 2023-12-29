using Demo.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UnitOfWorkContextRepository.Extensions;
using UnitOfWorkContextRepository.Repository;
using UnitOfWorkContextRepository.Transaction;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace UnitOfWorkContextRepository.Test.Fixtures
{
    public class RepositoryFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; }

        public RepositoryFixture()
        {
            var services = new ServiceCollection();

            var connection = CreateMemoryConnection();
            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                builder.UseSqlite(connection);
            });

            services.AddTransient(typeof(IEfCoreRepository<>), typeof(EfCoreRepository<>));
            services.AddTransient(typeof(IEfCoreRepository), typeof(EfCoreRepository<ApplicationDbContext>));
            services.AddTransient(typeof(IDbContextProvider<>), typeof(DefaultDbContextProvider<>));

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
