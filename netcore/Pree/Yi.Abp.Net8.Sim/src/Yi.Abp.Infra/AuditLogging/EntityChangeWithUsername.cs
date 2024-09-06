using Yi.Abp.Infra.AuditLogging.Entities;

namespace Yi.Abp.Infra.AuditLogging;

public class EntityChangeWithUsername
{
    public EntityChangeEntity EntityChange { get; set; }

    public string UserName { get; set; }
}
