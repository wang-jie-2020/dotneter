using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Extensions.DependencyInjection;

namespace Yi.AspNetCore.Mvc.ExceptionHandling;

public class ExceptionFilter : IAsyncExceptionFilter, ITransientDependency
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

        if (context.Exception is UnauthorizedException)
        {
            var defaultLocalizer = context.GetRequiredService<IStringLocalizer>();

            context.HttpContext.Response.StatusCode = context.HttpContext.User.Identity!.IsAuthenticated
                ? StatusCodes.Status403Forbidden
                : StatusCodes.Status401Unauthorized;
            
            context.Result = new ObjectResult(AjaxResult.Error(defaultLocalizer["UnauthorizedMessage"]));
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(errorInfo);
        }

        context.ExceptionHandled = true;
    }

    protected void LogException(ExceptionContext context, out AjaxResult errorInfo)
    {
        var exceptionToErrorInfoConverter = context.GetRequiredService<ExceptionToErrorInfoConverter>();
        errorInfo = exceptionToErrorInfoConverter.Convert(context.Exception);

        var logger = context.GetService<ILogger<ExceptionFilter>>(NullLogger<ExceptionFilter>.Instance);
        logger.LogException(context.Exception);
    }
}