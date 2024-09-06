using Microsoft.Extensions.DependencyInjection;
using Yi.Abp.Infra.AuditLogging.Repositories;
using Yi.Framework.SqlSugarCore;

namespace Yi.Abp.Infra.AuditLogging
{
    [DependsOn(
        typeof(YiFrameworkAuditLoggingDomainModule),

        typeof(YiFrameworkSqlSugarCoreModule))]
    public class YiFrameworkAuditLoggingSqlSugarCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IAuditLogRepository, SqlSugarCoreAuditLogRepository>();

        }
    }
}
