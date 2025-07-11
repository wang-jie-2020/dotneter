using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Tracing;
using Yi.AspNetCore.MultiTenancy;
using Yi.AspNetCore.Security;

namespace Yi.AspNetCore.Auditing;

public class AuditingHelper : IAuditingHelper, ITransientDependency
{
    protected ILogger<AuditingHelper> Logger { get; }

    protected IAuditingStore AuditingStore { get; }

    protected ICurrentUser CurrentUser { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected AuditingOptions Options;

    protected IServiceProvider ServiceProvider;

    protected ICorrelationIdProvider CorrelationIdProvider { get; }

    public AuditingHelper(
        IOptions<AuditingOptions> options,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        IAuditingStore auditingStore,
        ILogger<AuditingHelper> logger,
        IServiceProvider serviceProvider,
        ICorrelationIdProvider correlationIdProvider)
    {
        Options = options.Value;
        CurrentUser = currentUser;
        CurrentTenant = currentTenant;
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
            ClientId = "",
            CorrelationId = CorrelationIdProvider.Get(),
            ExecutionTime = DateTime.Now
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

            return JsonConvert.SerializeObject(dictionary);
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
