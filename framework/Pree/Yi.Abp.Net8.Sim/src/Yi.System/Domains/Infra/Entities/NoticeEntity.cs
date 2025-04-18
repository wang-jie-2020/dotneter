﻿using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Yi.Sys.Domains.Infra.Entities;

[SugarTable("Sys_Notice")]
public class NoticeEntity : AggregateRoot<Guid>, ISoftDelete, IAuditedObject
{
    [SugarColumn(IsPrimaryKey = true)] 
    public override Guid Id { get; protected set; }

    /// <summary>
    ///     公告标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    ///     类型
    /// </summary>
    public NoticeTypeEnum Type { get; set; }

    /// <summary>
    ///     内容
    /// </summary>
    [SugarColumn(ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string Content { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? LastModifierId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public int OrderNum { get; set; }

    public bool IsDeleted { get; set; }
    
    public bool State { get; set; }
}