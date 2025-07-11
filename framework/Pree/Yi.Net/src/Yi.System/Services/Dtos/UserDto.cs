﻿using Yi.Framework.Core.Entities;

namespace Yi.System.Services.Dtos;

public class UserDto
{
    /// <summary>
    ///     主键
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     逻辑删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    ///     姓名
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     年龄
    /// </summary>
    public int? Age { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///     头像
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    ///     昵称
    /// </summary>
    public string? Nick { get; set; }

    /// <summary>
    ///     邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     Ip
    /// </summary>
    public string? Ip { get; set; }

    /// <summary>
    ///     地址
    /// </summary>

    public string? Address { get; set; }

    /// <summary>
    ///     电话
    /// </summary>
    public long? Phone { get; set; }

    /// <summary>
    ///     简介
    /// </summary>
    public string? Introduction { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    ///     性别
    /// </summary>
    public SexEnum Sex { get; set; } = SexEnum.Unknown;

    /// <summary>
    ///     部门id
    /// </summary>
    public Guid? DeptId { get; set; }

    /// <summary>
    ///     创建时间
    /// </summary>
    public DateTime CreationTime { get; set; } = DateTime.Now;

    /// <summary>
    ///     创建者
    /// </summary>
    public Guid? CreatorId { get; set; }

    /// <summary>
    ///     最后修改者
    /// </summary>
    public Guid? LastModifierId { get; set; }

    /// <summary>
    ///     最后修改时间
    /// </summary>
    public DateTime? LastModificationTime { get; set; }

    /// <summary>
    ///     排序
    /// </summary>
    public int OrderNum { get; set; } = 0;
    
    /// <summary>
    ///     状态
    /// </summary>
    public bool State { get; set; } = true;
}