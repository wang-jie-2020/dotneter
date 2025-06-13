using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Tracing;
using Volo.Abp.Users;
using Yi.AspNetCore.MultiTenancy;

namespace Volo.Abp.Auditing;

public class AuditingHelper : IAuditingHelper, ITransientDependency
{
    protected ILogger<AuditingHelper> Logger { get; }
    protected IAuditingStore AuditingStore { get; }
    protected ICurrentUser CurrentUser { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected ICurrentClient CurrentClient { get; }

    protected AbpAuditingOptions Options;
    protected IAuditSerializer AuditSerializer;
    protected IServiceProvider ServiceProvider;
    protected ICorrelationIdProvider CorrelationIdProvider { get; }

    public AuditingHelper(
        IAuditSerializer auditSerializer,
        IOptions<AbpAuditingOptions> options,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        ICurrentClient currentClient,
        IAuditingStore auditingStore,
        ILogger<AuditingHelper> logger,
        IServiceProvider serviceProvider,
        ICorrelationIdProvider correlationIdProvider)
    {
        Options = options.Value;
        AuditSerializer = auditSerializer;
        CurrentUser = currentUser;
        CurrentTenant = currentTenant;
        CurrentClient = currentClient;
        AuditingStore = auditingStore;

        Logger = logger;
        ServiceProvider = serviceProvider;
        CorrelationIdProvider = correlationIdProvider;
    }

    public virtual bool ShouldSaveAudit(MethodInfo? methodInfo, bool defaultValue = false, bool ignoreIntegrationServiceAttribute = false)
    {
        if (methodInfo == null)
        {
            return false;
        }

        if (!methodInfo.IsPublic)
        {
            return false;
        }

        return defaultValue;
    }

    public virtual bool IsEntityHistoryEnabled(Type entityType, bool defaultValue = false)
    {
        if (!entityType.IsPublic)
        {
            return false;
        }

        return defaultValue;
    }

    public virtual AuditLogInfo CreateAuditLogInfo()
    {
        var auditInfo = new AuditLogInfo
        {
            ApplicationName = Options.ApplicationName,
            TenantId = CurrentTenant.Id,
            TenantName = CurrentTenant.Name,
            UserId = CurrentUser.Id,
            UserName = CurrentUser.UserName,
            ClientId = CurrentClient.Id,
            CorrelationId = CorrelationIdProvider.Get(),
            ExecutionTime = DateTime.Now,
            ImpersonatorUserId = CurrentUser.FindImpersonatorUserId(),
            ImpersonatorUserName = CurrentUser.FindImpersonatorUserName(),
            ImpersonatorTenantId = CurrentUser.FindImpersonatorTenantId(),
            ImpersonatorTenantName = CurrentUser.FindImpersonatorTenantName(),
        };

        ExecutePreContributors(auditInfo);

        return auditInfo;
    }

    public virtual AuditLogActionInfo CreateAuditLogAction(
        AuditLogInfo auditLog,
        Type? type,
        MethodInfo method,
        object?[] arguments)
    {
        return CreateAuditLogAction(auditLog, type, method, CreateArgumentsDictionary(method, arguments));
    }

    public virtual AuditLogActionInfo CreateAuditLogAction(
        AuditLogInfo auditLog,
        Type? type,
        MethodInfo method,
        IDictionary<string, object?> arguments)
    {
        var actionInfo = new AuditLogActionInfo
        {
            ServiceName = type != null
                ? type.FullName!
                : "",
            MethodName = method.Name,
            Parameters = SerializeConvertArguments(arguments),
            ExecutionTime = DateTime.Now
        };

        //TODO Execute contributors

        return actionInfo;
    }

    protected virtual void ExecutePreContributors(AuditLogInfo auditLogInfo)
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = new AuditLogContributionContext(scope.ServiceProvider, auditLogInfo);

            foreach (var contributor in Options.Contributors)
            {
                try
                {
                    contributor.PreContribute(context);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, LogLevel.Warning);
                }
            }
        }
    }

    protected virtual string SerializeConvertArguments(IDictionary<string, object?> arguments)
    {
        try
        {
            if (arguments.IsNullOrEmpty())
            {
                return "{}";
            }

            var dictionary = new Dictionary<string, object?>();

            foreach (var argument in arguments)
            {
                dictionary[argument.Key] = argument.Value;
            }

            return AuditSerializer.Serialize(dictionary);
        }
        catch (Exception ex)
        {
            Logger.LogException(ex, LogLevel.Warning);
            return "{}";
        }
    }

    protected virtual Dictionary<string, object?> CreateArgumentsDictionary(MethodInfo method, object?[] arguments)
    {
        var parameters = method.GetParameters();
        var dictionary = new Dictionary<string, object?>();

        for (var i = 0; i < parameters.Length; i++)
        {
            dictionary[parameters[i].Name!] = arguments[i];
        }

        return dictionary;
    }
}
