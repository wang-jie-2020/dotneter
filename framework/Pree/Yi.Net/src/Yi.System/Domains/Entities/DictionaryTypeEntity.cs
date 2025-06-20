using Yi.Framework.Abstractions;

namespace Yi.System.Domains.Entities;

[SugarTable("Sys_DictionaryType")]
public class DictionaryTypeEntity : BizEntity<Guid>
{
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

    /// <summary>
    ///     排序
    /// </summary>
    public int OrderNum { get; set; } = 0;
}