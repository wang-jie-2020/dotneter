using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Http;

namespace Yi.AspNetCore.System.Exceptions;

public class YiExceptionFilter : AbpExceptionFilter, IAsyncExceptionFilter
{
    private AjaxResult _error;

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        await base.OnExceptionAsync(context);
    }

    protected override async Task HandleAndWrapException(ExceptionContext context)
    {
        await base.HandleAndWrapException(context);
        context.Result = new ObjectResult(_error);
    }

    protected override void LogException(ExceptionContext context, out RemoteServiceErrorInfo remoteServiceErrorInfo)
    {
        base.LogException(context, out remoteServiceErrorInfo);
        _error = AjaxResult.Error(remoteServiceErrorInfo.Code ?? "1", remoteServiceErrorInfo.Message, remoteServiceErrorInfo.Details);
    }
}