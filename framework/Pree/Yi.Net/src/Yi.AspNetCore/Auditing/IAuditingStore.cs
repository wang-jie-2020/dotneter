namespace Yi.AspNetCore.Auditing;

public interface IAuditingStore
{
    Task SaveAsync(AuditLogInfo auditInfo);
}
