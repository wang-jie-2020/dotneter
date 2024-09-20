using SqlSugar;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Yi.AspNetCore.SqlSugarCore.Entities;

public class BizEntity<T> : SimpleEntity<T>, ISoftDelete, IAuditedObject
{
    public BizEntity()
    {
        
    }

    public BizEntity(T id) : base(id)
    {
        
    }

    public bool IsDeleted { get; set; }

    public DateTime CreationTime { get; set; }

    public Guid? CreatorId { get; set; }

    public DateTime? LastModificationTime { get; set; }

    public Guid? LastModifierId { get; set; }
}