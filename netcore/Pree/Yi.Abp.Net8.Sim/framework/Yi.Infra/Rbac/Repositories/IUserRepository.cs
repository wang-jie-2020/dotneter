using Yi.Framework.SqlSugarCore.Abstractions;
using Yi.Infra.Rbac.Entities;

namespace Yi.Infra.Rbac.Repositories
{
    public interface IUserRepository : ISqlSugarRepository<UserAggregateRoot>
    {
        /// <summary>
        /// 获取用户的所有信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserAggregateRoot> GetUserAllInfoAsync(Guid userId);
        /// <summary>
        /// 批量获取用户的所有信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<List<UserAggregateRoot>> GetListUserAllInfoAsync(List<Guid> userIds);

    }
}
