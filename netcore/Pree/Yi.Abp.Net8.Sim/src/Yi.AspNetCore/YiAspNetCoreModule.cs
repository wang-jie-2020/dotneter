using System.Reflection;
using FreeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using SqlSugar;
using Volo.Abp.Application;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Yi.AspNetCore.Caching.FreeRedis;
using Yi.AspNetCore.Mapster;
using Yi.AspNetCore.SqlSugarCore;
using Yi.AspNetCore.SqlSugarCore.Repositories;
using Yi.AspNetCore.SqlSugarCore.Uow;

namespace Yi.AspNetCore;

[DependsOn(
    typeof(AbpDddApplicationModule),
    typeof(AbpDddDomainModule),
    typeof(AbpObjectMappingModule),
    typeof(AbpCachingModule))]
public class YiAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        //Mapster
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
        context.Services.AddTransient(typeof(ISugarDbContextProvider<>), typeof(UnitOfWorkSqlsugarDbContextProvider<>));
        context.Services.AddTransient(typeof(ISqlSugarDbConnectionCreator), typeof(SqlSugarDbConnectionCreator));
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