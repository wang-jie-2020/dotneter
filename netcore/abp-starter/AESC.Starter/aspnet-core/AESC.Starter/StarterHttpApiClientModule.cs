namespace AESC.Starter
{
    [DependsOn(
        typeof(StarterApplicationContractsModule),
        typeof(BasicManagementHttpApiClientModule),
        typeof(NotificationManagementHttpApiClientModule),
        typeof(DataDictionaryManagementHttpApiClientModule),
        typeof(LanguageManagementHttpApiClientModule)
    )]
    public class StarterHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(StarterApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
