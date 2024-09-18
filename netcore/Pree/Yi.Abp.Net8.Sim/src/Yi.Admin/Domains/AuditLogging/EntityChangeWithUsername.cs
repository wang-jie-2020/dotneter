using Yi.Admin.Domains.AuditLogging.Entities;

namespace Yi.Admin.Domains.AuditLogging;

public class EntityChangeWithUsername
{
    public EntityChangeEntity EntityChange { get; set; }

    public string UserName { get; set; }
}