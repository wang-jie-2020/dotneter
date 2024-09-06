﻿namespace Yi.Abp.Infra.Rbac.Etos
{
    /// <summary>
    /// 用户创建的id
    /// </summary>
    public class UserCreateEventArgs
    {
        public UserCreateEventArgs(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; set; }
    }
}
