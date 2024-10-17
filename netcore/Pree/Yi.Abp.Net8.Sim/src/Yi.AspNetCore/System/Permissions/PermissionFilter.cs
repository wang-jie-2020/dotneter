using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Yi.AspNetCore.System.Permissions;

internal class PermissionFilter : ActionFilterAttribute
{
    private readonly IPermissionHandler _permissionHandler;

    public PermissionFilter(IPermissionHandler permissionHandler)
    {
        _permissionHandler = permissionHandler;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
        {
            return;
        }
        
        List<PermissionAttribute>? attributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
            .Where(a => a.GetType() == typeof(PermissionAttribute))
            .Select(x => x as PermissionAttribute)
            .ToList()!;
        
        //空对象直接返回
        if (attributes.Count == 0) return;

        var result = false;
        foreach (var attribute in attributes)
        {
            result = _permissionHandler.IsPass(attribute.Code);
            //存在有一个不满，直接跳出
            if (!result) break;
        }
        
        if (!result)
        {
            var error = new AjaxResult()
            {
                Code = 1,
                Type = "403",
                Message = "您无权限访问,请联系管理员申请",
                Details = $"您无权限访问该接口-{context.HttpContext.Request.Path.Value}"
            };

            var content = new ObjectResult(error)
            {
                StatusCode = 403
            };
            
            context.Result = content;
        }
    }
}