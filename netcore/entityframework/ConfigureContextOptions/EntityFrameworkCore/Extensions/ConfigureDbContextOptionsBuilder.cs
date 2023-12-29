using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.EntityFrameworkCore.Extensions
{
    public class ConfigureDbContextOptionsBuilder
    {
        public IServiceProvider ServiceProvider { get; }

        public DbContextOptionsBuilder DbContextOptionsBuilder { get; protected set; }

        public string ConnectionString { get; set; }

        public ConfigureDbContextOptionsBuilder(IServiceProvider serviceProvider, string connectionString)
        {
            ServiceProvider = serviceProvider;
            DbContextOptionsBuilder = new DbContextOptionsBuilder()
                .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>());
            ConnectionString = connectionString;
        }
    }

    public class ConfigureDbContextOptionsBuilder<TDbContext> : ConfigureDbContextOptionsBuilder
        where TDbContext : DbContext
    {
        public new DbContextOptionsBuilder<TDbContext> DbContextOptionsBuilder =>
            (DbContextOptionsBuilder<TDbContext>)base.DbContextOptionsBuilder;

        public ConfigureDbContextOptionsBuilder(IServiceProvider serviceProvider, string connectionString) : base(serviceProvider, connectionString)
        {
            base.DbContextOptionsBuilder = new DbContextOptionsBuilder<TDbContext>()
                .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>());
        }
    }
}
