namespace Yi.System.Rbac;

public class UserInfoCacheKey
{
    public UserInfoCacheKey(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; set; }

    public override string ToString()
    {
        return $"User:{UserId}";
    }
}