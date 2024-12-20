﻿using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Yi.AspNetCore.System;

namespace Yi.AspNetCore.Exceptions;

/// <summary>
///   <see cref="AbpExceptionFilter"/>
///     RemoteServiceErrorResponse -> AjaxResult
/// </summary>
public class YiExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        if (!ShouldHandleException(context))
        {
            LogException(context, out _);
            return;
        }

        await HandleAndWrapException(context);
    }
    
    protected virtual bool ShouldHandleException(ExceptionContext context)
    {
        if (context.ExceptionHandled)
        {
            return false;
        }

        if (context.ActionDescriptor.IsControllerAction() &&
            context.ActionDescriptor.HasObjectResult())
        {
            return true;
        }

        if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
        {
            return true;
        }

        if (context.HttpContext.Request.IsAjax())
        {
            return true;
        }

        return false;
    }

    protected async Task HandleAndWrapException(ExceptionContext context)
    {
        LogException(context, out var remoteServiceErrorInfo);

        await context.GetRequiredService<IExceptionNotifier>().NotifyAsync(new ExceptionNotificationContext(context.Exception));

        if (context.Exception is AbpAuthorizationException)
        {
            await context.HttpContext.RequestServices.GetRequiredService<IAbpAuthorizationExceptionHandler>()
                .HandleAsync(context.Exception.As<AbpAuthorizationException>(), context.HttpContext);
        }
        else
        {
            context.HttpContext.Response.Headers.Add(AbpHttpConsts.AbpErrorFormat, "true");
            context.HttpContext.Response.StatusCode = (int)context
                .GetRequiredService<IHttpExceptionStatusCodeFinder>()
                .GetStatusCode(context.HttpContext, context.Exception);

            //abp RemoteServiceErrorResponse
            //context.Result = new ObjectResult(new RemoteServiceErrorResponse(remoteServiceErrorInfo));
            
            //AjaxResult
            // if (context.Exception is IBusinessException)
            // {
            //     context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            // }
            context.Result = new ObjectResult(AjaxResult.Error(remoteServiceErrorInfo.Code, remoteServiceErrorInfo.Message, remoteServiceErrorInfo.Details));
        }

        context.ExceptionHandled = true; //Handled!
    }

    protected void LogException(ExceptionContext context, out RemoteServiceErrorInfo remoteServiceErrorInfo)
    {
        var exceptionHandlingOptions = context.GetRequiredService<IOptions<AbpExceptionHandlingOptions>>().Value;
        var exceptionToErrorInfoConverter = context.GetRequiredService<IExceptionToErrorInfoConverter>();
        remoteServiceErrorInfo = exceptionToErrorInfoConverter.Convert(context.Exception, options =>
        {
            options.SendExceptionsDetailsToClients = exceptionHandlingOptions.SendExceptionsDetailsToClients;
            options.SendStackTraceToClients = exceptionHandlingOptions.SendStackTraceToClients;
        });

        var remoteServiceErrorInfoBuilder = new StringBuilder();
        remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
        remoteServiceErrorInfoBuilder.AppendLine(context.GetRequiredService<IJsonSerializer>().Serialize(remoteServiceErrorInfo, indented: true));

        var logger = context.GetService<ILogger<AbpExceptionFilter>>(NullLogger<AbpExceptionFilter>.Instance)!;
        var logLevel = context.Exception.GetLogLevel();
        logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());
        logger.LogException(context.Exception, logLevel);
    }
}