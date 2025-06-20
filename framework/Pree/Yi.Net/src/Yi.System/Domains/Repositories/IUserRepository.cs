﻿using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Domains.Entities;

namespace Yi.System.Domains.Repositories;

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