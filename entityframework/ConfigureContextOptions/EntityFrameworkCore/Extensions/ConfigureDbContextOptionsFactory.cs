using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Demo.EntityFrameworkCore.Extensions
{
    public static class ConfigureDbContextOptionsFactory
    {
        public static DbContextOptions<TDbContext> CreateContextOptions<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : DbContext
        {
            var accessor = GetDbContextCreationAccessor<TDbContext>(serviceProvider);

            var options = serviceProvider.GetRequiredService<IOptions<ConfigureDbContextOptions>>().Value;
            var builder = new ConfigureDbContextOptionsBuilder<TDbContext>(serviceProvider, accessor.ConnectionString);

            var configureAction = options.ConfigureActions
                .TryGetValue(typeof(TDbContext), out object obj) ? obj : default;
            if (configureAction != null)
            {
                ((Action<ConfigureDbContextOptionsBuilder<TDbContext>>)configureAction).Invoke(builder);
            }
            else if (options.ConfigureAction != null)
            {
                options.ConfigureAction.Invoke(builder);
            }
            else
            {
                throw new Exception($"No configuration found for {typeof(DbContext).AssemblyQualifiedName}!");
            }

            return builder.DbContextOptionsBuilder.Options;
        }

        private static ConfigureDbContextCreationAccessor GetDbContextCreationAccessor<TDbContext>(IServiceProvider serviceProvider)
        {
            var accessor = ConfigureDbContextCreationAccessor.Current;
            if (accessor != null)
            {
                return accessor;
            }

            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionString = serviceProvider
                .GetRequiredService<IConnectionStringResolver>()
                .Resolve(connectionStringName);

            return new ConfigureDbContextCreationAccessor(connectionString);
        }
    }
}
