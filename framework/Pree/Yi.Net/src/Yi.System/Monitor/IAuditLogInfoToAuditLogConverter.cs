using Yi.Framework.Auditing;
using Yi.System.Monitor.Entities;

namespace Yi.System.Monitor;

public interface IAuditLogInfoToAuditLogConverter
{
    Task<AuditLogEntity> ConvertAsync(AuditLogInfo auditLogInfo);
}