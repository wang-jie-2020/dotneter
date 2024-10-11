using Yi.System.Domains.Sys.Entities;

namespace Yi.System.Domains.Sys.Repositories;

public interface IUserRepository : ISqlSugarRepository<UserEntity>
{
    /// <summary>
    ///     获取用户的所有信息
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<UserEntity> GetUserAllInfoAsync(Guid userId);

    /// <summary>
    ///     批量获取用户的所有信息
    /// </summary>
    /// <param name="userIds"></param>
    /// <returns></returns>
    Task<List<UserEntity>> GetListUserAllInfoAsync(List<Guid> userIds);
}