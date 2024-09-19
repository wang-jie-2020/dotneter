using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Yi.AspNetCore.Common;

public class OperationLogFilter : ActionFilterAttribute, ITransientDependency
{
    private readonly ILogger<OperationLogFilter> _logger;
    private readonly IOperationLogStore _operationLogStore;
    private readonly ICurrentUser _currentUser;
    
    public OperationLogFilter(
        ILogger<OperationLogFilter> logger,
        IOperationLogStore operationLogStore,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _operationLogStore = operationLogStore;
        _currentUser = currentUser;
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var resultContext = await next.Invoke();

        if (resultContext.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
        {
            return;
        }

        //查找标签，获取标签对象
        var operLogAttribute = controllerActionDescriptor
            .MethodInfo.GetCustomAttributes(true)
            .FirstOrDefault(a => a.GetType().Equals(typeof(OperationLogAttribute))) as OperationLogAttribute;

        //空对象直接返回
        if (operLogAttribute is null)
        {
            return;
        }
        
        var info = new OperationLogInfo
        {
            Title = operLogAttribute.Title,
            OperationLog = operLogAttribute.OperLogType,
            Operator = _currentUser.UserName,
            Method = resultContext.HttpContext.Request.Path.Value,
            RequestMethod = resultContext.HttpContext.Request.Method,
        };
        
        if (operLogAttribute.IsSaveResponseData)
        {
            if (resultContext.Result is ContentResult result && result.ContentType == "application/json")
            {
                info.Result = result.Content?.Replace("\r\n", "").Trim();
            }

            if (resultContext.Result is JsonResult result2)
            {
                info.Result = result2.Value?.ToString();
            }

            if (resultContext.Result is ObjectResult result3)
            {
                info.Result = JsonConvert.SerializeObject(result3.Value);
            }
        }

        if (operLogAttribute.IsSaveRequestData)
        {
            //不建议保存，吃性能
            //info.RequestParam = context.HttpContext.GetRequestValue(logEntity.RequestMethod);
        }

        await _operationLogStore.SaveAsync(info);
    }
}