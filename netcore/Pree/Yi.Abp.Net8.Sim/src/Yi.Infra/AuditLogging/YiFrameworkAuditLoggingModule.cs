using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.Domain;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.AuditLogging.Repositories;

namespace Yi.Infra.AuditLogging;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpDddDomainSharedModule),
    typeof(AbpAuditingModule),
    typeof(YiFrameworkSqlSugarCoreModule)
)]
public class YiFrameworkAuditLoggingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IAuditLogRepository, SqlSugarCoreAuditLogRepository>();
    }
}