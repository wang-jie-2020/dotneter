namespace AESC.Starter
{
    [DependsOn(
        typeof(BasicManagementDomainSharedModule),
        typeof(NotificationManagementDomainSharedModule),
        typeof(DataDictionaryManagementDomainSharedModule),
        typeof(LanguageManagementDomainSharedModule),
        typeof(AbpProCoreModule)
    )]
    public class StarterDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            StarterGlobalFeatureConfigurator.Configure();
            StarterModuleExtensionConfigurator.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<StarterDomainSharedModule>(StarterDomainSharedConsts.NameSpace);
            });
          
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<StarterResource>(StarterDomainSharedConsts.DefaultCultureName)
                    .AddVirtualJson("/Localization/Starter")
                    .AddBaseTypes(typeof(BasicManagementResource))
                    .AddBaseTypes(typeof(AbpTimingResource));

                options.DefaultResourceType = typeof(StarterResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace(StarterDomainSharedConsts.NameSpace, typeof(StarterResource));
            });
        }

       
    }
}