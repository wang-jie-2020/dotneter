﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Extensions.DependencyInjection;

namespace Yi.AspNetCore.Auditing;

public class AuditActionFilter : IAsyncActionFilter, ITransientDependency
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!ShouldSaveAudit(context, out var auditLog, out var auditLogAction))
        {
            await next();
            return;
        }

        using (AbpCrossCuttingConcerns.Applying(context.Controller, AbpCrossCuttingConcerns.Auditing))
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var result = await next();

                if (result.Exception != null && !auditLog!.Exceptions.Contains(result.Exception))
                {
                    auditLog!.Exceptions.Add(result.Exception);
                }
            }
            catch (Exception ex)
            {
                if (!auditLog!.Exceptions.Contains(ex))
                {
                    auditLog!.Exceptions.Add(ex);
                }
                throw;
            }
            finally
            {
                stopwatch.Stop();

                if (auditLogAction != null)
                {
                    auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                    auditLog!.Actions.Add(auditLogAction);
                }
            }
        }
    }

    private bool ShouldSaveAudit(ActionExecutingContext context, out AuditLogInfo? auditLog, out AuditLogActionInfo? auditLogAction)
    {
        auditLog = null;
        auditLogAction = null;

        var options = context.GetRequiredService<IOptions<AuditingOptions>>().Value;
        if (!options.IsEnabled)
        {
            return false;
        }

        if (!context.ActionDescriptor.IsControllerAction())
        {
            return false;
        }

        var auditLogScope = context.GetRequiredService<IAuditingManager>().Current;
        if (auditLogScope == null)
        {
            return false;
        }

        var auditingHelper = context.GetRequiredService<IAuditingHelper>();
        if (!auditingHelper.ShouldSaveAudit(
                context.ActionDescriptor.GetMethodInfo(),
                defaultValue: GetDefaultAuditBehavior(options, context.ActionDescriptor)))
        {
            return false;
        }

        auditLog = auditLogScope.Log;
        auditLogAction = auditingHelper.CreateAuditLogAction(
            auditLog,
            context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.AsType(),
            context.ActionDescriptor.AsControllerActionDescriptor().MethodInfo,
            context.ActionArguments
        );
        
        return true;
    }

    private static bool GetDefaultAuditBehavior(
        AuditingOptions auditingOptions,
        ActionDescriptor actionDescriptor)
    {
        return true;
    }
}
