using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Yi.Sys.Domains.Infra.Entities;

[SugarTable("Sys_DictionaryType")]
public class DictionaryTypeEntity : AggregateRoot<Guid>, IAuditedObject, ISoftDelete
{
    /// <summary>
    ///     主键
    /// </summary>
    [SugarColumn(IsPrimaryKey = true)]
    public override Guid Id { get; protected set; }
    
    /// <summary>
    ///     状态
    /// </summary>
    public bool? State { get; set; } = true;

    /// <summary>
    ///     字典名称
    /// </summary>
    [SugarColumn(ColumnName = "DictName")]
    public string DictName { get; set; } = string.Empty;

    /// <summary>
    ///     字典类型
    /// </summary>
    [SugarColumn(ColumnName = "DictType")]
    public string DictType { get; set; } = string.Empty;

    /// <summary>
    ///     描述
    /// </summary>
    [SugarColumn(ColumnName = "Remark")]
    public string? Remark { get; set; }

    [SugarColumn(IsIgnore = true)] public override ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? LastModifierId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    /// <summary>
    ///     排序
    /// </summary>
    public int OrderNum { get; set; } = 0;

    /// <summary>
    ///     逻辑删除
    /// </summary>
    public bool IsDeleted { get; set; }
}