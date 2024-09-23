﻿using Yi.System.Domains.System.Entities;

namespace Yi.System.Domains.System.Repositories;

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