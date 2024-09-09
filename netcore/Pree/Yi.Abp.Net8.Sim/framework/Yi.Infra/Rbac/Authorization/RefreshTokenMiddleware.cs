﻿using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Yi.Infra.Rbac.Consts;
using Yi.Infra.Rbac.Managers;

namespace Yi.Infra.Rbac.Authorization;

[DebuggerStepThrough]
public class RefreshTokenMiddleware : IMiddleware, ITransientDependency
{
    private readonly AccountManager _accountManager;

    public RefreshTokenMiddleware(AccountManager accountManager)
    {
        _accountManager = accountManager;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var refreshToken = context.Request.Headers["refresh_token"].ToString();
        if (!string.IsNullOrEmpty(refreshToken))
        {
            //每个用户的token刷新频率可以进行控制，防止刷新token当访问token使用
            var authResult = await context.AuthenticateAsync(TokenTypeConst.Refresh);
            //token鉴权刷新成功
            if (authResult.Succeeded)
            {
                var userId = Guid.Parse(authResult.Principal.FindFirst(AbpClaimTypes.UserId).Value);
                var access_Token = await _accountManager.GetTokenByUserIdAsync(userId);
                var refresh_Token = _accountManager.CreateRefreshToken(userId);
                context.Response.Headers["access_token"] = access_Token;
                context.Response.Headers["refresh_token"] = refresh_Token;


                //请求头替换，补充后续鉴权逻辑
                context.Request.Headers["Authorization"] = "Bearer " + access_Token;
            }
        }

        await next(context);
    }
}

public static class RefreshTokenExtensions
{
    public static IApplicationBuilder UseRefreshToken([NotNull] this IApplicationBuilder app)
    {
        app.UseMiddleware<RefreshTokenMiddleware>();
        return app;
    }
}