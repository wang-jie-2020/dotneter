using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Auditing;
using Yi.Admin.Domains.AuditLogging;
using Yi.Admin.Domains.AuditLogging.Repositories;
using Yi.Admin.Domains.OperLogging;
using Yi.AspNetCore;
using Yi.AspNetCore.System;
using Yi.AspNetCore.System.Loggings;

namespace Yi.Admin;

[DependsOn(typeof(YiAspNetCoreModule))]
public class YiAdminModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Auditing
        context.Services.Replace(new ServiceDescriptor(typeof(IAuditingStore), typeof(AuditingStore), ServiceLifetime.Singleton));
        context.Services.AddTransient<IAuditLogRepository, SqlSugarCoreAuditLogRepository>();
        context.Services.AddTransient<IAuditLogInfoToAuditLogConverter, AuditLogInfoToAuditLogConverter>();
        
        //OperationLog
        context.Services.Replace(new ServiceDescriptor(typeof(IOperLogStore), typeof(OperLogStore), ServiceLifetime.Singleton));
    }
}