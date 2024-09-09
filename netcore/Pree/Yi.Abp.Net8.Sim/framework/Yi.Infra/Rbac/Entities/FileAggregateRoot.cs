using SqlSugar;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Yi.Infra.Rbac.Entities;

[SugarTable("File")]
public class FileAggregateRoot : AggregateRoot<Guid>, IAuditedObject
{
    public FileAggregateRoot()
    {
    }

    public FileAggregateRoot(Guid fileId)
    {
        Id = fileId;
    }

    [SugarColumn(IsPrimaryKey = true)] public override Guid Id { get; protected set; }

    /// <summary>
    ///     文件大小
    /// </summary>
    [SugarColumn(ColumnName = "FileSize")]
    public decimal FileSize { get; set; }

    /// <summary>
    ///     文件名
    /// </summary>
    [SugarColumn(ColumnName = "FileName")]
    public string FileName { get; set; }

    /// <summary>
    ///     文件路径
    /// </summary>
    [SugarColumn(ColumnName = "FilePath")]
    public string FilePath { get; set; }

    [SugarColumn(IsIgnore = true)] public override ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public DateTime CreationTime { get; set; }
    public Guid? CreatorId { get; set; }

    public Guid? LastModifierId { get; set; }

    public DateTime? LastModificationTime { get; set; }
}