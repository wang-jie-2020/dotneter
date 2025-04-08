using Volo.Abp.Auditing;
using Yi.Sys.Domains.Monitor.Entities;

namespace Yi.Sys.Domains.Monitor;

public interface IAuditLogInfoToAuditLogConverter
{
    Task<AuditLogEntity> ConvertAsync(AuditLogInfo auditLogInfo);
}