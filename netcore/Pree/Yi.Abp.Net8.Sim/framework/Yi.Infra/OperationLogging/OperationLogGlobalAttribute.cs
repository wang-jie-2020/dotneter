﻿using IPTools.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Yi.Framework.Core.Extensions;
using Yi.Infra.OperationLogging.Entities;

namespace Yi.Infra.OperationLogging;

public class OperationLogGlobalAttribute : ActionFilterAttribute, ITransientDependency
{
    private readonly ICurrentUser _currentUser;
    private ILogger<OperationLogGlobalAttribute> _logger;

    private readonly IRepository<OperationLogEntity> _repository;

    //注入一个日志服务
    public OperationLogGlobalAttribute(ILogger<OperationLogGlobalAttribute> logger, IRepository<OperationLogEntity> repository,
        ICurrentUser currentUser)
    {
        _logger = logger;
        _repository = repository;
        _currentUser = currentUser;
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var resultContext = await next.Invoke();
        //执行后

        //判断标签是在方法上
        if (resultContext.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

        //查找标签，获取标签对象
        var operLogAttribute = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
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

        var logEntity = new OperationLogEntity();
        logEntity.OperIp = ip;
        //logEntity.OperLocation = location;
        logEntity.OperType = operLogAttribute.OperType;
        logEntity.Title = operLogAttribute.Title;
        logEntity.RequestMethod = resultContext.HttpContext.Request.Method;
        logEntity.Method = resultContext.HttpContext.Request.Path.Value;
        logEntity.OperLocation = location;
        logEntity.OperUser = _currentUser.UserName;
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