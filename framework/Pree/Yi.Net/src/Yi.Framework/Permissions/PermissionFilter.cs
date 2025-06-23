using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Authorization;
using Yi.AspNetCore.Extensions.DependencyInjection;

namespace Yi.Framework.Permissions;

internal class PermissionFilter : IAsyncActionFilter, ITransientDependency
{
    private readonly IPermissionHandler _permissionHandler;

    public PermissionFilter(IPermissionHandler permissionHandler)
    {
        _permissionHandler = permissionHandler;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionDescriptor.IsControllerAction())
        {
            await next();
            return;
        }

        var methodInfo = context.ActionDescriptor.GetMethodInfo();

        var attributes = methodInfo.GetCustomAttributes(true)
            .OfType<PermissionAttribute>()
            .ToArray();

        if (attributes.Length > 0)
        {
            foreach (var attribute in attributes)
            {
                if (!_permissionHandler.IsPass(attribute.Code))
                {
                    throw new UnauthorizedException()
                        .WithData("Path", context.HttpContext.Request.Path);
                }
            }
        }

        await next();
    }
}