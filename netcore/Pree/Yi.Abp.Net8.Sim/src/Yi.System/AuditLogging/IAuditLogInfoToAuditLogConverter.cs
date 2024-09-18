using Volo.Abp.Auditing;
using Yi.System.AuditLogging.Entities;

namespace Yi.System.AuditLogging;

public interface IAuditLogInfoToAuditLogConverter
{
    Task<AuditLogAggregateRoot> ConvertAsync(AuditLogInfo auditLogInfo);
}