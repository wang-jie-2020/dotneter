namespace AESC.Starter.Data
{
    /* This is used if database provider does't define
     * IStarterDbSchemaMigrator implementation.
     */
    public class NullStarterDbSchemaMigrator : IStarterDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}