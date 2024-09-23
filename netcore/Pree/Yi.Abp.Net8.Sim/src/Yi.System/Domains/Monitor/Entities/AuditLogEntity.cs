﻿using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;
using Yi.AspNetCore.System.Entities;
using Yi.System.Domains.Monitor.Consts;

namespace Yi.System.Domains.Monitor.Entities;

[DisableAuditing]
[SugarTable("Sys_AuditLog")]
[SugarIndex($"index_{nameof(ExecutionTime)}", nameof(TenantId), OrderByType.Asc, nameof(ExecutionTime),
    OrderByType.Asc)]
[SugarIndex($"index_{nameof(ExecutionTime)}_{nameof(UserId)}", nameof(TenantId), OrderByType.Asc, nameof(UserId),
    OrderByType.Asc, nameof(ExecutionTime), OrderByType.Asc)]
public class AuditLogEntity : SimpleEntity<Guid>, IMultiTenant
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
        Guid? impersonatorUserId,
        string impersonatorUserName,
        Guid? impersonatorTenantId,
        string impersonatorTenantName,
        List<EntityChangeEntity> entityChanges,
        List<AuditLogActionEntity> actions,
        string exceptions,
        string comments)
    {
        Id = id;
        ApplicationName = applicationName.Truncate(AuditLogConsts.MaxApplicationNameLength);
        TenantId = tenantId;
        TenantName = tenantName.Truncate(AuditLogConsts.MaxTenantNameLength);
        UserId = userId;
        UserName = userName.Truncate(AuditLogConsts.MaxUserNameLength);
        ExecutionTime = executionTime;
        ExecutionDuration = executionDuration;
        ClientIpAddress = clientIpAddress.Truncate(AuditLogConsts.MaxClientIpAddressLength);
        ClientName = clientName.Truncate(AuditLogConsts.MaxClientNameLength);
        ClientId = clientId.Truncate(AuditLogConsts.MaxClientIdLength);
        CorrelationId = correlationId.Truncate(AuditLogConsts.MaxCorrelationIdLength);
        BrowserInfo = browserInfo.Truncate(AuditLogConsts.MaxBrowserInfoLength);
        HttpMethod = httpMethod.Truncate(AuditLogConsts.MaxHttpMethodLength);
        Url = url.Truncate(AuditLogConsts.MaxUrlLength);
        HttpStatusCode = httpStatusCode;
        ImpersonatorUserId = impersonatorUserId;
        ImpersonatorUserName = impersonatorUserName.Truncate(AuditLogConsts.MaxUserNameLength);
        ImpersonatorTenantId = impersonatorTenantId;
        ImpersonatorTenantName = impersonatorTenantName.Truncate(AuditLogConsts.MaxTenantNameLength);
        EntityChanges = entityChanges;
        Actions = actions;
        Exceptions = exceptions;
        Comments = comments.Truncate(AuditLogConsts.MaxCommentsLength);
    }
    
    public virtual string? ApplicationName { get; set; }

    public virtual Guid? UserId { get; protected set; }

    public virtual string? UserName { get; protected set; }

    public virtual string? TenantName { get; protected set; }

    public virtual Guid? ImpersonatorUserId { get; protected set; }

    public virtual string? ImpersonatorUserName { get; protected set; }

    public virtual Guid? ImpersonatorTenantId { get; protected set; }

    public virtual string? ImpersonatorTenantName { get; protected set; }

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
    [Navigate(NavigateType.OneToMany, nameof(EntityChangeEntity.AuditLogId))]
    public virtual List<EntityChangeEntity> EntityChanges { get; protected set; }

    //导航属性
    [Navigate(NavigateType.OneToMany, nameof(AuditLogActionEntity.AuditLogId))]
    public virtual List<AuditLogActionEntity> Actions { get; protected set; }
    
    public virtual Guid? TenantId { get; protected set; }
}