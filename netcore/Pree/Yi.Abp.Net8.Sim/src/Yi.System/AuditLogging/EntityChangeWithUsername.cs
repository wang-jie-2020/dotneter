using Yi.System.AuditLogging.Entities;

namespace Yi.System.AuditLogging;

public class EntityChangeWithUsername
{
    public EntityChangeEntity EntityChange { get; set; }

    public string UserName { get; set; }
}