using Yi.System.Domains.AuditLogging.Entities;

namespace Yi.System.Domains.AuditLogging;

public class EntityChangeWithUsername
{
    public EntityChangeEntity EntityChange { get; set; }

    public string UserName { get; set; }
}