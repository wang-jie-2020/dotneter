using Yi.AspNetCore.Auditing;
using Yi.AspNetCore.Utils;
using Yi.System.Monitor.Entities;

namespace Yi.System.Monitor;

public class AuditLogInfoToAuditLogConverter : IAuditLogInfoToAuditLogConverter
{
    public AuditLogInfoToAuditLogConverter()
    {
    }

    public virtual Task<AuditLogEntity> ConvertAsync(AuditLogInfo auditLogInfo)
    {
        var auditLogId = SequentialGuidGenerator.Create();

        var actions = auditLogInfo.Actions?
            .Select(auditLogActionInfo => new AuditLogActionEntity(SequentialGuidGenerator.Create(), auditLogId,
                auditLogActionInfo, auditLogInfo.TenantId))
            .ToList() ?? new List<AuditLogActionEntity>();

        var comments = auditLogInfo
            .Comments?
            .JoinAsString(Environment.NewLine);

        var auditLog = new AuditLogEntity(
            auditLogId,
            auditLogInfo.ApplicationName,
            auditLogInfo.TenantId,
            auditLogInfo.TenantName,
            auditLogInfo.UserId,
            auditLogInfo.UserName,
            auditLogInfo.ExecutionTime,
            auditLogInfo.ExecutionDuration,
            auditLogInfo.ClientIpAddress,
            auditLogInfo.ClientName,
            auditLogInfo.ClientId,
            auditLogInfo.CorrelationId,
            auditLogInfo.BrowserInfo,
            auditLogInfo.HttpMethod,
            auditLogInfo.Url,
            auditLogInfo.HttpStatusCode,
            auditLogInfo.ImpersonatorUserId,
            auditLogInfo.ImpersonatorUserName,
            auditLogInfo.ImpersonatorTenantId,
            auditLogInfo.ImpersonatorTenantName,
            actions,
            string.Empty,
            comments
        );

        return Task.FromResult(auditLog);
    }
}