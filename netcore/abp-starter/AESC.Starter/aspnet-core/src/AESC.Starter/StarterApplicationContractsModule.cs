namespace AESC.Starter
{
    [DependsOn(
        typeof(StarterDomainSharedModule),
        typeof(AbpObjectExtendingModule),
        typeof(BasicManagementApplicationContractsModule),
        typeof(NotificationManagementApplicationContractsModule),
        typeof(DataDictionaryManagementApplicationContractsModule),
        typeof(LanguageManagementApplicationContractsModule)
    )]
    public class StarterApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            StarterDtoExtensions.Configure();
        }
    }
}
