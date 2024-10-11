using Yi.Sys.Domains.Monitor.Entities;

namespace Yi.Sys.Domains.Monitor;

public class EntityChangeWithUsername
{
    public EntityChangeEntity EntityChange { get; set; }

    public string UserName { get; set; }
}