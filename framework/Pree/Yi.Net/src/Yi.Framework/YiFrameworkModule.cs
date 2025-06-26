using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SkyApm;
using StackExchange.Profiling.Internal;
using Yi.AspNetCore;
using Yi.Framework.Auditing;
using Yi.Framework.Loggings;
using Yi.Framework.Permissions;
using Yi.Framework.SqlSugarCore;
using Yi.Framework.SqlSugarCore.Profilers;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.Framework.SqlSugarCore.Uow;
using Yitter.IdGenerator;

namespace Yi.Framework;

[DependsOn(typeof(YiAspNetCoreModule))]
public class YiFrameworkModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(OperLogInterceptorRegistrar.RegisterIfNeeded);
    }
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        // AspNetCore & Mvc
        context.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.AddService<PermissionFilter>();
            options.Filters.AddService<OperLogFilter>();
            options.Filters.AddService<AuditActionFilter>();
        });

        // Interceptors
        context.Services.AddTransient<OperLogInterceptor>();

        // SqlSugar
        Configure<DbConnOptions>(configuration.GetSection("DbConnOptions"));

        context.Services.AddTransient(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
        context.Services.AddTransient(typeof(ISugarDbContextProvider<>), typeof(UnitOfWorkSqlSugarDbContextProvider<>));
        context.Services.AddTransient(typeof(ISqlSugarDbConnectionCreator), typeof(SqlSugarDbConnectionCreator));

        context.Services.AddSingleton<IMiniProfilerDiagnosticListener, SqlSugarDiagnosticListener>();
        context.Services.AddSingleton<ITracingDiagnosticProcessor, SqlSugarTracingDiagnosticProcessor>();

        // Other
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions(0));
        Configure<AuditingOptions>(options =>
        {
            options.Contributors.Add(new AspNetCoreAuditLogContributor());
        });
    }
}