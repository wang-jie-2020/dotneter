using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.Domain;
using Yi.Abp.Infra.AuditLogging.Repositories;
using Yi.Framework.SqlSugarCore;

namespace Yi.Abp.Infra.AuditLogging;

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