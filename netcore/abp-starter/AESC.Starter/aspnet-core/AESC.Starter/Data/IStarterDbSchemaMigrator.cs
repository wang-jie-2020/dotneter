namespace AESC.Starter.Data
{
    public interface IStarterDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
