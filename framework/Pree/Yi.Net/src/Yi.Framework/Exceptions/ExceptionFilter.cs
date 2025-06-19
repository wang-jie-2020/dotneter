using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Yi.AspNetCore.Core;
using Yi.AspNetCore.Extensions;
using Yi.AspNetCore.Mvc.Core;

namespace Yi.AspNetCore.Exceptions;

public class ExceptionFilter : IAsyncExceptionFilter
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

        return false;
    }

    protected async Task HandleAndWrapException(ExceptionContext context)
    {
        LogException(context, out var errorInfo);
        
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(errorInfo);
        
        context.ExceptionHandled = true; //Handled!
    }

    protected void LogException(ExceptionContext context, out AjaxResult errorInfo)
    {
        var exceptionToErrorInfoConverter = context.GetRequiredService<ExceptionToErrorInfoConverter>();
        errorInfo = exceptionToErrorInfoConverter.Convert(context.Exception);
        
        var logger = context.GetService<ILogger<ExceptionFilter>>();
        logger.LogException(context.Exception);
    }
}