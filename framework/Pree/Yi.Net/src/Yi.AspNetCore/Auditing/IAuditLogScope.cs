namespace Yi.AspNetCore.Auditing;

public interface IAuditLogScope
{
    AuditLogInfo Log { get; }
}
