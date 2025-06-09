using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Yi.System.Domains.Entities;

/// <summary>
///     配置表
/// </summary>
[SugarTable("Sys_Config")]
public class ConfigEntity : AggregateRoot<Guid>, IAuditedObject, ISoftDelete
{
    [SugarColumn(ColumnName = "Id", IsPrimaryKey = true)]
    public override Guid Id { get; protected set; }

    /// <summary>
    ///     配置名称
    /// </summary>
    [SugarColumn(ColumnName = "ConfigName")]
    public string ConfigName { get; set; } = string.Empty;

    /// <summary>
    ///     配置键
    /// </summary>
    [SugarColumn(ColumnName = "ConfigKey")]
    public string ConfigKey { get; set; } = string.Empty;

    /// <summary>
    ///     配置值
    /// </summary>
    [SugarColumn(ColumnName = "ConfigValue")]
    public string ConfigValue { get; set; } = string.Empty;

    /// <summary>
    ///     配置类别
    /// </summary>
    [SugarColumn(ColumnName = "ConfigType")]
    public string? ConfigType { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    [SugarColumn(ColumnName = "Remark")]
    public string? Remark { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? LastModifierId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    /// <summary>
    ///     排序字段
    /// </summary>
    [SugarColumn(ColumnName = "OrderNum")]
    public int OrderNum { get; set; }

    public bool IsDeleted { get; set; }
}