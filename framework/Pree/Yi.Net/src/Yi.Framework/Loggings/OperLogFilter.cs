using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Security;

namespace Yi.Framework.Loggings;

public class OperLogFilter : ActionFilterAttribute, ITransientDependency
{
    private readonly ILogger<OperLogFilter> _logger;
    private readonly IOperLogStore _operLogStore;
    private readonly ICurrentUser _currentUser;

    public OperLogFilter(
        ILogger<OperLogFilter> logger,
        IOperLogStore operLogStore,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _operLogStore = operLogStore;
        _currentUser = currentUser;
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var resultContext = await next.Invoke();

        if (resultContext.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
        {
            return;
        }
        
        var operLogAttribute = controllerActionDescriptor
            .MethodInfo.GetCustomAttributes(true)
            .OfType<OperLogAttribute>()
            .FirstOrDefault();
        
        if (operLogAttribute == null)
        {
            return;
        }

        var info = new OperLogInfo
        {
            Title = operLogAttribute.Title,
            OperLog = operLogAttribute.OperLogType,
            Operator = _currentUser.UserName,
            Method = resultContext.HttpContext.Request.Path.Value,
            RequestMethod = resultContext.HttpContext.Request.Method,
            ExecutionTime = DateTime.Now
        };

        if (operLogAttribute.IsSaveResponseData)
        {
            try
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
            catch
            {
                // ignored
            }
        }

        if (operLogAttribute.IsSaveRequestData)
        {
            //不建议保存，吃性能
            //info.RequestParam = context.HttpContext.GetRequestValue(logEntity.RequestMethod);
        }

        await _operLogStore.SaveAsync(info);
    }
}