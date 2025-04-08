using Volo.Abp.Modularity;
using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    [DependsOn(
        typeof(AbpMultiStadiumModule),
        typeof(AbpAspNetCoreModule)
        )]
    public class AbpAspNetCoreMultiStadiumModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpStadiumResolveOptions>(options =>
            {
                options.StadiumResolvers.Add(new QueryStringStadiumResolveContributor());
                options.StadiumResolvers.Add(new FormStadiumResolveContributor());
                options.StadiumResolvers.Add(new RouteStadiumResolveContributor());
                options.StadiumResolvers.Add(new HeaderStadiumResolveContributor());
                options.StadiumResolvers.Add(new CookieStadiumResolveContributor());
            });
        }
    }
}
