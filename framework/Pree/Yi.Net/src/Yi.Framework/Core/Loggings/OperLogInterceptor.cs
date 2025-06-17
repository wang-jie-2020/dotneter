using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Users;

namespace Yi.AspNetCore.Core.Loggings;

public class OperLogInterceptor : AbpInterceptor
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

        var attrs = invocation.Method.GetCustomAttributes(true).OfType<OperLogAttribute>().ToArray();

        if (attrs.IsNullOrEmpty())
        {
            return;
        }

        var operLogAttribute = attrs.FirstOrDefault();

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