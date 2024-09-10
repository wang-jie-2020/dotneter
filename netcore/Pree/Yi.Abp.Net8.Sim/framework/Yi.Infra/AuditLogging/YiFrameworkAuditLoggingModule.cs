using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.Domain;
using Yi.Framework;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.AuditLogging.Repositories;

namespace Yi.Infra.AuditLogging;

[DependsOn(typeof(AbpAuditingModule), typeof(YiAspNetCoreModule))]
public class YiFrameworkAuditLoggingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient<IAuditingStore, AuditingStore>();
        context.Services.AddTransient<IAuditLogRepository, SqlSugarCoreAuditLogRepository>();
        context.Services.AddTransient<IAuditLogInfoToAuditLogConverter, AuditLogInfoToAuditLogConverter>();
    }
}