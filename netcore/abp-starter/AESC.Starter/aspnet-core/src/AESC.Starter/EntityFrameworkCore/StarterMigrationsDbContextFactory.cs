namespace AESC.Starter.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class StarterMigrationsDbContextFactory : IDesignTimeDbContextFactory<StarterDbContext>
    {
        public StarterDbContext CreateDbContext(string[] args)
        {
            StarterEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<StarterDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"), MySqlServerVersion.LatestSupportedServerVersion);

            return new StarterDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath
                (
                    Path.Combine
                    (
                        Directory.GetCurrentDirectory(),
                        "../AESC.Starter.DbMigrator/"
                    )
                )
                .AddJsonFile
                (
                    "appsettings.json",
                    false
                );

            return builder.Build();
        }
    }
}