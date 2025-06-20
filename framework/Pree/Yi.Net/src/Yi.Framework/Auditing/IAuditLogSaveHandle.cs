namespace Yi.Framework.Auditing;

public interface IAuditLogSaveHandle : IDisposable
{
    Task SaveAsync();
}
