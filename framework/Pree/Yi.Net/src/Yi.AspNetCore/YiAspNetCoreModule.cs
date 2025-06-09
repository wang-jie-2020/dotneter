using System.Reflection;
using FreeRedis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SkyApm;
using SqlSugar;
using StackExchange.Profiling.Internal;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Yi.AspNetCore.Caching.FreeRedis;
using Yi.AspNetCore.Core.Loggings;
using Yi.AspNetCore.Core.Permissions;
using Yi.AspNetCore.Exceptions;
using Yi.AspNetCore.Mapster;
using Yi.AspNetCore.SqlSugarCore;
using Yi.AspNetCore.SqlSugarCore.Profilers;
using Yi.AspNetCore.SqlSugarCore.Repositories;
using Yi.AspNetCore.SqlSugarCore.Uow;
using Yitter.IdGenerator;
using SequentialGuidGenerator = Yi.AspNetCore.Helpers.SequentialGuidGenerator;

namespace Yi.AspNetCore;

[DependsOn(
    typeof(AbpAuditingModule),
    typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpDddDomainModule),
    typeof(AbpDddDomainSharedModule),
    typeof(AbpObjectMappingModule)
)]
public class YiAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(OperLogInterceptorRegistrar.RegisterIfNeeded);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        //Mapster
        context.Services.AddTransient<IObjectMapper, MapsterObjectMapper>();
        context.Services.AddTransient<IAutoObjectMappingProvider, MapsterAutoObjectMappingProvider>();

        //Redis
        var redisEnabled = configuration["Redis:IsEnabled"];
        if (redisEnabled.IsNullOrEmpty() || bool.Parse(redisEnabled))
        {
            var redisConfiguration = configuration["Redis:Configuration"];
            var redisClient = new RedisClient(redisConfiguration);

            context.Services.AddSingleton<IRedisClient>(redisClient);
            context.Services.Replace(ServiceDescriptor.Singleton<IDistributedCache>(new DistributedCache(redisClient)));
            context.Services.Replace(ServiceDescriptor.Transient<IDistributedCacheKeyNormalizer, YiDistributedCacheKeyNormalizer>());
        }

        //SqlSugar
        Configure<DbConnOptions>(configuration.GetSection("DbConnOptions"));

        context.Services.TryAddScoped<ISqlSugarDbContext, SqlSugarDbContext>();
        context.Services.AddTransient(typeof(IRepository<>), typeof(SqlSugarRepository<>));
        context.Services.AddTransient(typeof(IRepository<,>), typeof(SqlSugarRepository<,>));
        context.Services.AddTransient(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
        context.Services.AddTransient(typeof(ISqlSugarRepository<,>), typeof(SqlSugarRepository<,>));
        context.Services.AddTransient(typeof(ISugarDbContextProvider<>), typeof(UnitOfWorkSqlSugarDbContextProvider<>));
        context.Services.AddTransient(typeof(ISqlSugarDbConnectionCreator), typeof(SqlSugarDbConnectionCreator));

        //AspNetCore
        context.Services.AddTransient<IPermissionHandler, DefaultPermissionHandler>();
        context.Services.AddTransient<PermissionFilter>();
        context.Services.AddSingleton<IOperLogStore, SimpleOperLogStore>();

        context.Services.AddTransient<YiExceptionFilter>();
        context.Services.Replace(ServiceDescriptor.Transient<IExceptionToErrorInfoConverter, YiExceptionToErrorInfoConverter>());
        context.Services.Configure<MvcOptions>(options =>
        {
            // 权限过滤器
            options.Filters.AddService<PermissionFilter>();
            
            // 操作日志过滤器
            options.Filters.AddService<OperLogFilter>();

            // 错误过滤器
            var abpExceptionFilter = options.Filters.FirstOrDefault(metadata => (metadata as ServiceFilterAttribute)?.ServiceType == typeof(AbpExceptionFilter));
            if (abpExceptionFilter != null)
            {
                options.Filters.Remove(abpExceptionFilter);
            }
            
            var abpExceptionPageFilter = options.Filters.FirstOrDefault(metadata => (metadata as ServiceFilterAttribute)?.ServiceType == typeof(AbpExceptionPageFilter));
            if (abpExceptionPageFilter != null)
            {
                options.Filters.Remove(abpExceptionPageFilter);
            }
            
            options.Filters.AddService<YiExceptionFilter>();
        });

        // 雪花Id
        YitIdHelper.SetIdGenerator(new IdGeneratorOptions(0));

        // uuid
        context.Services.AddTransient<IGuidGenerator, SequentialGuidGenerator>();

        // interceptor
        context.Services.AddTransient<OperLogInterceptor>();
        
        // profiler
        context.Services.AddSingleton<IMiniProfilerDiagnosticListener, SqlSugarDiagnosticListener>();
        context.Services.AddSingleton<ITracingDiagnosticProcessor, SqlSugarTracingDiagnosticProcessor>();
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