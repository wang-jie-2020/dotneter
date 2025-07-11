using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Yi.AspNetCore.Security;

namespace Yi.AspNetCore.Mvc.OperLogging;

public class OperLogInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly ILogger<OperLogInterceptor> _logger;
    private readonly IOperLogStore _operLogStore;
    private readonly ICurrentUser _currentUser;

    public OperLogInterceptor(
        ILogger<OperLogInterceptor> logger,
        IOperLogStore operLogStore,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _operLogStore = operLogStore;
        _currentUser = currentUser;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        await invocation.ProceedAsync();

        var operLogAttribute = invocation.Method.GetCustomAttributes(true).OfType<OperLogAttribute>().FirstOrDefault();

        if (operLogAttribute == null)
        {
            return;
        }

        var info = new OperLogInfo
        {
            Title = operLogAttribute.Title,
            OperLog = operLogAttribute.OperLogType,
            Operator = _currentUser.UserName,
            Method = invocation.Method.Name,
            RequestMethod = invocation.Method.Name,
            ExecutionTime = DateTime.Now
        };

        if (operLogAttribute.IsSaveResponseData)
        {
            try
            {
                info.Result = JsonConvert.SerializeObject(invocation.ReturnValue);
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