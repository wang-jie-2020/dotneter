using Yi.System.Domains.Monitor.Entities;

namespace Yi.System.Domains.Monitor;

public class EntityChangeWithUsername
{
    public EntityChangeEntity EntityChange { get; set; }

    public string UserName { get; set; }
}