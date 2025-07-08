using Yi.AspNetCore.MultiTenancy;

namespace Yi.System.Monitor.Entities;

[SugarTable("Sys_AuditLog")]
[SugarIndex($"index_{nameof(ExecutionTime)}", nameof(TenantId), OrderByType.Asc, nameof(ExecutionTime),
    OrderByType.Asc)]
[SugarIndex($"index_{nameof(ExecutionTime)}_{nameof(UserId)}", nameof(TenantId), OrderByType.Asc, nameof(UserId),
    OrderByType.Asc, nameof(ExecutionTime), OrderByType.Asc)]
public class AuditLogEntity : Entity<Guid>, IMultiTenant
{
    public AuditLogEntity()
    {
    }

    public AuditLogEntity(
        Guid id,
        string applicationName,
        Guid? tenantId,
        string tenantName,
        Guid? userId,
        string userName,
        DateTime executionTime,
        int executionDuration,
        string clientIpAddress,
        string clientName,
        string clientId,
        string correlationId,
        string browserInfo,
        string httpMethod,
        string url,
        int? httpStatusCode,
        List<AuditLogActionEntity> actions,
        string exceptions,
        string comments)
    {
        Id = id;
        ApplicationName = applicationName;
        TenantId = tenantId;
        TenantName = tenantName;
        UserId = userId;
        UserName = userName;
        ExecutionTime = executionTime;
        ExecutionDuration = executionDuration;
        ClientIpAddress = clientIpAddress;
        ClientName = clientName;
        ClientId = clientId;
        CorrelationId = correlationId;
        BrowserInfo = browserInfo;
        HttpMethod = httpMethod;
        Url = url;
        HttpStatusCode = httpStatusCode;
        Actions = actions;
        Exceptions = exceptions;
        Comments = comments;
    }
    
    public virtual string? ApplicationName { get; set; }

    public virtual Guid? UserId { get; protected set; }

    public virtual string? UserName { get; protected set; }

    public virtual string? TenantName { get; protected set; }

    public virtual DateTime? ExecutionTime { get; protected set; }

    public virtual int? ExecutionDuration { get; protected set; }

    public virtual string? ClientIpAddress { get; protected set; }

    public virtual string? ClientName { get; protected set; }

    public virtual string? ClientId { get; set; }

    public virtual string? CorrelationId { get; set; }

    public virtual string? BrowserInfo { get; protected set; }

    public virtual string? HttpMethod { get; protected set; }

    public virtual string? Url { get; protected set; }

    public virtual string? Exceptions { get; protected set; }

    public virtual string? Comments { get; protected set; }

    public virtual int? HttpStatusCode { get; set; }

    //导航属性
    [Navigate(NavigateType.OneToMany, nameof(AuditLogActionEntity.AuditLogId))]
    public virtual List<AuditLogActionEntity> Actions { get; protected set; }
    
    public virtual Guid? TenantId { get; protected set; }
}