using IPTools.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Yi.System.OperationLogging.Entities;

namespace Yi.System.OperationLogging;

public class OperationLogGlobalAttribute : ActionFilterAttribute, ITransientDependency
{
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<OperationLogGlobalAttribute> _logger;
    private readonly IRepository<OperationLogEntity> _repository;

    public OperationLogGlobalAttribute(
        ILogger<OperationLogGlobalAttribute> logger,
        IRepository<OperationLogEntity> repository,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _repository = repository;
        _currentUser = currentUser;
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var resultContext = await next.Invoke();

        if (resultContext.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

        //查找标签，获取标签对象
        var operLogAttribute = controllerActionDescriptor
            .MethodInfo.GetCustomAttributes(true)
            .FirstOrDefault(a => a.GetType().Equals(typeof(OperationLogAttribute))) as OperationLogAttribute;

        //空对象直接返回
        if (operLogAttribute is null) return;

        ////获取控制器名
        //string controller = context.RouteData.Values["Controller"].ToString();

        ////获取方法名
        //string action = context.RouteData.Values["Action"].ToString();

        //获取Ip
        var ip = resultContext.HttpContext.GetClientIp();

        //根据ip获取地址
        var ipTool = IpTool.Search(ip);
        var location = ipTool.Province + " " + ipTool.City;

        //日志服务插入一条操作记录即可
        var logEntity = new OperationLogEntity
        {
            OperIp = ip,
            //logEntity.OperLocation = location;
            OperType = operLogAttribute.OperType,
            Title = operLogAttribute.Title,
            RequestMethod = resultContext.HttpContext.Request.Method,
            Method = resultContext.HttpContext.Request.Path.Value,
            OperLocation = location,
            OperUser = _currentUser.UserName
        };
        if (operLogAttribute.IsSaveResponseData)
        {
            if (resultContext.Result is ContentResult result && result.ContentType == "application/json")
                logEntity.RequestResult = result.Content?.Replace("\r\n", "").Trim();
            if (resultContext.Result is JsonResult result2) logEntity.RequestResult = result2.Value?.ToString();

            if (resultContext.Result is ObjectResult result3)
                logEntity.RequestResult = JsonConvert.SerializeObject(result3.Value);
        }

        if (operLogAttribute.IsSaveRequestData)
        {
            //不建议保存，吃性能
            //logEntity.RequestParam = context.HttpContext.GetRequestValue(logEntity.RequestMethod);
        }

        await _repository.InsertAsync(logEntity);
    }
}