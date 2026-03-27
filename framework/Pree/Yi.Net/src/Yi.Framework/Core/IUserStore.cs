namespace Yi.Framework.Core;

public interface IUserStore
{
    Task<UserConfiguration> GetInfoAsync(long userId, bool refreshCache = false);
}