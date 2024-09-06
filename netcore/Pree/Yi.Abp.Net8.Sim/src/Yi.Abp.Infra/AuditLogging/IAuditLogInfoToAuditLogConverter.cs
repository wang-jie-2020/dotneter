using Volo.Abp.Auditing;
using Yi.Abp.Infra.AuditLogging.Entities;

namespace Yi.Abp.Infra.AuditLogging;

public interface IAuditLogInfoToAuditLogConverter
{
    Task<AuditLogAggregateRoot> ConvertAsync(AuditLogInfo auditLogInfo);
}
