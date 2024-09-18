using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.SignalR;
using Volo.Abp.Auditing;
using Yi.System.Services.Account.Options;
using Yi.System.Services.OperationLogging;

namespace Yi.System;

[DependsOn(
    typeof(AbpAuditingModule),
    typeof(AbpAspNetCoreSignalRModule)
)]
public class YiInfraModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        
       
        
        //Rbac
        
        var service = context.Services;

        service.AddCaptcha();

        var configuration = context.Services.GetConfiguration();
        service.AddControllers(options =>
        {
            options.Filters.Add<OperationLogGlobalAttribute>();
        });

        //配置阿里云短信
        Configure<AliyunOptions>(configuration.GetSection(nameof(AliyunOptions)));

        Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        Configure<RefreshJwtOptions>(configuration.GetSection(nameof(RefreshJwtOptions)));
        Configure<RbacOptions>(configuration.GetSection(nameof(RbacOptions)));

        context.Services.TryAddYiDbContext<YiRbacDbContext>();
    }
}