using Yi.Infra.AuditLogging.Entities;

namespace Yi.Infra.AuditLogging;

public class EntityChangeWithUsername
{
    public EntityChangeEntity EntityChange { get; set; }

    public string UserName { get; set; }
}