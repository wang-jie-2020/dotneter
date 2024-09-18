using Volo.Abp.Auditing;
using Yi.System.Domains.AuditLogging.Entities;

namespace Yi.System.Domains.AuditLogging;

public interface IAuditLogInfoToAuditLogConverter
{
    Task<AuditLogAggregateRoot> ConvertAsync(AuditLogInfo auditLogInfo);
}