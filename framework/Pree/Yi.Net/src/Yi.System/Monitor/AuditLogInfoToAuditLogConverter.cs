using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Volo.Abp.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.Http;
using Yi.System.Monitor.Entities;

namespace Yi.System.Monitor;

public class AuditLogInfoToAuditLogConverter : IAuditLogInfoToAuditLogConverter
{
    public AuditLogInfoToAuditLogConverter(IGuidGenerator guidGenerator)
    {
        GuidGenerator = guidGenerator;
    }

    protected IGuidGenerator GuidGenerator { get; }

    public virtual Task<AuditLogEntity> ConvertAsync(AuditLogInfo auditLogInfo)
    {
        var auditLogId = GuidGenerator.Create();

        var actions = auditLogInfo.Actions?
            .Select(auditLogActionInfo => new AuditLogActionEntity(GuidGenerator.Create(), auditLogId,
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