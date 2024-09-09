using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Yi.Framework.Caching.FreeRedis;
using Yi.Framework.Ddd.Application;
using Yi.Framework.Mapster;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Authorization;
using Yi.Infra.Rbac.Operlog;
using Yi.Infra.Rbac.Options;

namespace Yi.Infra.Rbac;

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
    typeof(YiFrameworkSqlSugarCoreModule)
)]
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