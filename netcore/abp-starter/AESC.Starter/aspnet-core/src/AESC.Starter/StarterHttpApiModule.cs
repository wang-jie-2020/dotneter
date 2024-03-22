namespace AESC.Starter
{
    [DependsOn(
        typeof(StarterApplicationContractsModule),
        typeof(BasicManagementHttpApiModule),
        typeof(NotificationManagementHttpApiModule),
        typeof(DataDictionaryManagementHttpApiModule),
        typeof(LanguageManagementHttpApiModule)
        )]
    public class StarterHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<StarterResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
