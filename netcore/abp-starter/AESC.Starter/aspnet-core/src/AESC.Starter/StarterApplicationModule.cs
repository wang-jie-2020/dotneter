namespace AESC.Starter
{
    [DependsOn(
        typeof(StarterDomainModule),
        typeof(StarterApplicationContractsModule),
        typeof(BasicManagementApplicationModule),
        typeof(NotificationManagementApplicationModule),
        typeof(DataDictionaryManagementApplicationModule),
        typeof(LanguageManagementApplicationModule),
        typeof(StarterFreeSqlModule)
        )]
    public class StarterApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<StarterApplicationModule>();
            });
            
        }
    }
}
