﻿using Microsoft.Extensions.DependencyInjection;
using Yi.AspNetCore;
using Yi.System.Options;

namespace Yi.System;

[DependsOn(typeof(YiAspNetCoreModule))]
public class YiInfraModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        //Rbac
        context.Services.AddCaptcha();

        Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        Configure<RefreshJwtOptions>(configuration.GetSection(nameof(RefreshJwtOptions)));
        Configure<RbacOptions>(configuration.GetSection(nameof(RbacOptions)));

        context.Services.TryAddYiDbContext<YiRbacDbContext>();
    }
}