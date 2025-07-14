namespace Yi.Framework.Core;

public interface IUserStore
{
    Task<UserConfiguration> GetInfoAsync(Guid userId, bool refreshCache = false);
}