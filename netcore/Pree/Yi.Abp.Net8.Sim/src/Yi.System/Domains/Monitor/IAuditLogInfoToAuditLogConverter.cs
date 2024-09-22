using Volo.Abp.Auditing;
using Yi.System.Domains.Monitor.Entities;

namespace Yi.System.Domains.Monitor;

public interface IAuditLogInfoToAuditLogConverter
{
    Task<AuditLogEntity> ConvertAsync(AuditLogInfo auditLogInfo);
}