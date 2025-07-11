﻿namespace Yi.Framework.SqlSugarCore;

public abstract class BizEntity<T> : Entity<T>, ISoftDelete, IBizEntity
{
    public BizEntity()
    {

    }

    public BizEntity(T id) : base(id)
    {

    }

    /// <summary>
    ///     逻辑删除
    /// </summary>
    public bool IsDeleted { get; set; }

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
}