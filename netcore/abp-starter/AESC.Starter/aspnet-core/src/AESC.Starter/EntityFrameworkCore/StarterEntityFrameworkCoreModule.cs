namespace AESC.Starter.EntityFrameworkCore
{
    [DependsOn(
        typeof(StarterDomainModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(BasicManagementEntityFrameworkCoreModule),
        typeof(DataDictionaryManagementEntityFrameworkCoreModule),
        typeof(NotificationManagementEntityFrameworkCoreModule),
        typeof(LanguageManagementEntityFrameworkCoreModule)
    )]
    public class StarterEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            StarterEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<StarterDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });
            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also StarterMigrationsDbContextFactory for EF Core tooling. */
                options.UseMySQL();
            });
        }
    }
}