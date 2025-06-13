namespace Yi.AspNetCore.Auditing;

public interface IAuditingManager
{
    IAuditLogScope? Current { get; }

    IAuditLogSaveHandle BeginScope();
}
