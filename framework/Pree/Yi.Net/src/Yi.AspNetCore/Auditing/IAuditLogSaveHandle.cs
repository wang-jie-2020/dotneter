namespace Yi.AspNetCore.Auditing;

public interface IAuditLogSaveHandle : IDisposable
{
    Task SaveAsync();
}
