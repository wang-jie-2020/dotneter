using Yi.Framework.Mapster;
using Yi.Framework.SqlSugarCore;

namespace Yi.Abp.Infra.Rbac
{
    [DependsOn(
        typeof(YiFrameworkRbacDomainModule),

        typeof(YiFrameworkMapsterModule),
        typeof(YiFrameworkSqlSugarCoreModule)
        )]
    public class YiFrameworkRbacSqlSugarCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.TryAddYiDbContext<YiRbacDbContext>();
        }
    }
}