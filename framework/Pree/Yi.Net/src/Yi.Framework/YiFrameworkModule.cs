using System.Reflection;
using FreeRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SkyApm;
using SqlSugar;
using StackExchange.Profiling.Internal;
using Volo.Abp.Autofac;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Yi.AspNetCore.Auditing;
using Yi.AspNetCore.Core.Filters;
using Yi.AspNetCore.Core.Loggings;
using Yi.AspNetCore.Core.Permissions;
using Yi.AspNetCore.Data.Filtering;
using Yi.AspNetCore.Data.Seeding;
using Yi.AspNetCore.Exceptions;
using Yi.AspNetCore.MultiTenancy;
using Yi.AspNetCore.SqlSugarCore;
using Yi.AspNetCore.SqlSugarCore.Profilers;
using Yi.AspNetCore.SqlSugarCore.Repositories;
using Yi.AspNetCore.SqlSugarCore.Uow;
using Yitter.IdGenerator;

namespace Yi.AspNetCore;

public class YiFrameworkModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(OperLogInterceptorRegistrar.RegisterIfNeeded);

        AutoAddDataSeedContributors(context.Services);
    }

    private static void AutoAddDataSeedContributors(IServiceCollection services)
    {
        var contributors = new List<Type>();

        services.OnRegistered(context =>
        {
            if (typeof(IDataSeedContributor).IsAssignableFrom(context.ImplementationType))
            {
                contributors.Add(context.ImplementationType);
            }
        });

        services.Configure<DataSeedOptions>(options =>
        {
            options.Contributors.AddIfNotContains(contributors);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        //SqlSugar
        Configure<DbConnOptions>(configuration.GetSection("DbConnOptions"));

        context.Services.TryAddScoped<ISqlSugarDbContext, SqlSugarDbContext>();
        context.Services.AddTransient(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
        context.Services.AddTransient(typeof(ISqlSugarRepository<,>), typeof(SqlSugarRepository<,>));
        context.Services.AddTransient(typeof(ISugarDbContextProvider<>), typeof(UnitOfWorkSqlSugarDbContextProvider<>));
        context.Services.AddTransient(typeof(ISqlSugarDbConnectionCreator), typeof(SqlSugarDbConnectionCreator));

        //AspNetCore
        context.Services.AddObjectAccessor<IApplicationBuilder>();
        context.Services.AddHttpContextAccessor();
        Configure<AuditingOptions>(options =>
        {
            options.Contributors.Add(new AspNetCoreAuditLogContributor());
        });

        context.Services.AddTransient<IPermissionHandler, DefaultPermissionHandler>();
        context.Services.AddTransient<PermissionFilter>();
        context.Services.AddSingleton<IOperLogStore, SimpleOperLogStore>();

        context.Services.AddTransient<ExceptionFilter>();
        context.Services.AddTransient<ExceptionToErrorInfoConverter>();

        context.Services.AddMvc()
            .AddDataAnnotationsLocalization().AddViewLocalization()
            .AddControllersAsServices().AddViewComponentsAsServices();
        context.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

        context.Services.Configure<MvcOptions>(options =>
        {
            // 权限过滤器
            options.Filters.AddService<PermissionFilter>();

            // 操作日志过滤器
            options.Filters.AddService<OperLogFilter>();

            // 错误过滤器
            //var abpExceptionFilter = options.Filters.FirstOrDefault(metadata => (metadata as ServiceFilterAttribute)?.ServiceType == typeof(AbpExceptionFilter));
            //if (abpExceptionFilter != null)
            //{
            //    options.Filters.Remove(abpExceptionFilter);
            //}
            options.Filters.AddService<ExceptionFilter>();

            options.Filters.AddService<UowActionFilter>();
            options.Filters.AddService<AuditActionFilter>();
        });

        // 雪花Id
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions(0));

        // interceptor
        context.Services.AddTransient<OperLogInterceptor>();

        // profiler
        context.Services.AddSingleton<IMiniProfilerDiagnosticListener, SqlSugarDiagnosticListener>();
        context.Services.AddSingleton<ITracingDiagnosticProcessor, SqlSugarTracingDiagnosticProcessor>();


        context.Services.AddSingleton<ICurrentTenantAccessor>(AsyncLocalCurrentTenantAccessor.Instance);
        context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));

        context.Services.AddSingleton(typeof(IAmbientScopeProvider<>), typeof(AmbientDataContextAmbientScopeProvider<>));
    }

    public override async Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var service = context.ServiceProvider;
        var options = service.GetRequiredService<IOptions<DbConnOptions>>().Value;

        // var sb = new StringBuilder();
        // sb.AppendLine();
        // sb.AppendLine("==========Yi-SQL配置:==========");
        // sb.AppendLine($"数据库连接字符串：{options.Url}");
        // sb.AppendLine($"数据库类型：{options.DbType.ToString()}");
        // sb.AppendLine($"是否开启种子数据：{options.EnabledDbSeed}");
        // sb.AppendLine($"是否开启CodeFirst：{options.EnabledCodeFirst}");
        // sb.AppendLine($"是否开启Saas多租户：{options.EnabledSaasMultiTenancy}");
        // sb.AppendLine("===============================");
        // var logger = service.GetRequiredService<ILogger<YiFrameworkSqlSugarCoreModule>>();
        // logger.LogInformation(sb.ToString());

        if (options.EnabledCodeFirst) CodeFirst(service);
        if (options.EnabledDbSeed) await DataSeedAsync(service);
    }

    private void CodeFirst(IServiceProvider service)
    {
        var moduleContainer = service.GetRequiredService<IModuleContainer>();
        var db = service.GetRequiredService<ISqlSugarDbContext>().SqlSugarClient;

        //尝试创建数据库
        db.DbMaintenance.CreateDatabase();

        var types = new List<Type>();
        foreach (var module in moduleContainer.Modules)
            types.AddRange(module.Assembly.GetTypes()
                .Where(x => x.GetCustomAttribute<IgnoreCodeFirstAttribute>() == null)
                .Where(x => x.GetCustomAttribute<SugarTable>() != null)
                .Where(x => x.GetCustomAttribute<SplitTableAttribute>() is null));
        if (types.Count > 0) db.CopyNew().CodeFirst.InitTables(types.ToArray());
    }

    private async Task DataSeedAsync(IServiceProvider service)
    {
        var dataSeeder = service.GetRequiredService<IDataSeeder>();
        await dataSeeder.SeedAsync();
    }
}