using Volo.Abp.Auditing;
using Yi.Infra.AuditLogging.Entities;

namespace Yi.Infra.AuditLogging;

public interface IAuditLogInfoToAuditLogConverter
{
    Task<AuditLogAggregateRoot> ConvertAsync(AuditLogInfo auditLogInfo);
}
