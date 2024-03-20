namespace AESC.Starter.EntityFrameworkCore
{
    public class EntityFrameworkCoreStarterDbSchemaMigrator
        : IStarterDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreStarterDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the StarterMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<StarterDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}