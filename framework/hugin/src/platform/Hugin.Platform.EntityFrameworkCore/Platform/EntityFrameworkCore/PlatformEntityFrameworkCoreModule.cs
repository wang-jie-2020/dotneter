using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Hugin.Platform.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class PlatformEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PlatformDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}