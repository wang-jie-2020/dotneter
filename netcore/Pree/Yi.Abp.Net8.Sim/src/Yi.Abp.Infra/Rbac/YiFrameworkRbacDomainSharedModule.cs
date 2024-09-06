using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Yi.Abp.Infra.Rbac.Options;
using Yi.Framework.Mapster;

namespace Yi.Abp.Infra.Rbac
{
    [DependsOn(typeof(AbpDddDomainSharedModule),
        typeof(YiFrameworkMapsterModule)
        )]
    public class YiFrameworkRbacDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            Configure<RefreshJwtOptions>(configuration.GetSection(nameof(RefreshJwtOptions)));
            Configure<RbacOptions>(configuration.GetSection(nameof(RbacOptions)));
        }
    }
}