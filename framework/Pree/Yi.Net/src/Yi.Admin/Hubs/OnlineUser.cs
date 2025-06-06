namespace Yi.Admin.Hubs;

public class OnlineUser
{
    public OnlineUser()
    {
    }

    public OnlineUser(string connectionId)
    {
        ConnectionId = connectionId;
    }

    /// <summary>
    ///     客户端连接Id
    /// </summary>
    public string? ConnectionId { get; }

    /// <summary>
    ///     用户id
    /// </summary>
    public Guid? UserId { get; set; }

    public string? UserName { get; set; }
    
    public DateTime LoginTime { get; set; }
    
    public string? Ipaddr { get; set; }
    
    public string? LoginLocation { get; set; }
    
    public string? Os { get; set; }
    
    public string? Browser { get; set; }
}