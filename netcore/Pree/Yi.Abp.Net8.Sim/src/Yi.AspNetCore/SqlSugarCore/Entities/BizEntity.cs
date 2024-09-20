using SqlSugar;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Yi.AspNetCore.SqlSugarCore.Entities;

public class BizEntity<T> : IEntity<T>, ISoftDelete, IAuditedObject
{
    [SugarColumn(ColumnName = "Id", IsPrimaryKey = true)]
    public T Id { get; set; }

    public object?[] GetKeys()
    {
        return new object?[] { Id };
    }

    public bool IsDeleted { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }
}