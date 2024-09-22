using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;
using Yi.AspNetCore;
using Yi.System.Domains.TenantManagement;
using Yi.System.Options;

namespace Yi.System;

[DependsOn(typeof(YiAspNetCoreModule))]
public class YiSystemModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        //Tenant
        context.Services.Replace(new ServiceDescriptor(typeof(ITenantStore), typeof(SqlSugarAndConfigurationTenantStore), ServiceLifetime.Transient));
        context.Services.Replace(new ServiceDescriptor(typeof(IConnectionStringResolver), typeof(YiMultiTenantConnectionStringResolver), ServiceLifetime.Transient));
        context.Services.Replace(new ServiceDescriptor(typeof(ITenantConfigurationProvider), typeof(YiTenantConfigurationProvider), ServiceLifetime.Transient));
        
        
        context.Services.AddCaptcha();

        Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        Configure<RefreshJwtOptions>(configuration.GetSection(nameof(RefreshJwtOptions)));
        Configure<RbacOptions>(configuration.GetSection(nameof(RbacOptions)));

        context.Services.TryAddYiDbContext<YiDataScopedDbContext>();
    }
}