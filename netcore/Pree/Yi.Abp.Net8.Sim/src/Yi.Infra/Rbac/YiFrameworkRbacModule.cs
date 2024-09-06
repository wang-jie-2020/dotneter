using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Yi.Abp.Infra.Rbac.Authorization;
using Yi.Abp.Infra.Rbac.Operlog;
using Yi.Abp.Infra.Rbac.Options;
using Yi.Framework.Caching.FreeRedis;
using Yi.Framework.Ddd.Application;
using Yi.Framework.Ddd.Application.Contracts;
using Yi.Framework.Mapster;
using Yi.Framework.SqlSugarCore;

namespace Yi.Abp.Infra.Rbac;

[DependsOn(
    typeof(YiFrameworkCachingFreeRedisModule),
    typeof(AbpAspNetCoreSignalRModule),
    typeof(AbpDddDomainModule),
    typeof(AbpCachingModule),
    typeof(YiFrameworkDddApplicationModule),
    typeof(AbpBackgroundWorkersQuartzModule),
    typeof(AbpDddDomainSharedModule),
    typeof(YiFrameworkMapsterModule),
    typeof(YiFrameworkMapsterModule),
    typeof(YiFrameworkSqlSugarCoreModule),
    typeof(YiFrameworkDddApplicationContractsModule))]
public class YiFrameworkRbacModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var service = context.Services;

        service.AddCaptcha();

        var configuration = context.Services.GetConfiguration();
        service.AddControllers(options =>
        {
            options.Filters.Add<PermissionGlobalAttribute>();
            options.Filters.Add<OperLogGlobalAttribute>();
        });

        //配置阿里云短信
        Configure<AliyunOptions>(configuration.GetSection(nameof(AliyunOptions)));

        Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        Configure<RefreshJwtOptions>(configuration.GetSection(nameof(RefreshJwtOptions)));
        Configure<RbacOptions>(configuration.GetSection(nameof(RbacOptions)));

        context.Services.TryAddYiDbContext<YiRbacDbContext>();
    }
}