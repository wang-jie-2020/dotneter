using Volo.Abp.Auditing;
using Volo.Abp.Guids;
using Yi.AspNetCore.Core.Entities;
using Yi.AspNetCore.MultiTenancy;
using Yi.System.Monitor.Consts;

namespace Yi.System.Monitor.Entities;

[SugarTable("Sys_EntityPropertyChange")]
[SugarIndex($"index_{nameof(EntityChangeId)}", nameof(EntityChangeId), OrderByType.Asc)]
public class EntityPropertyChangeEntity : SimpleEntity<Guid>, IMultiTenant
{
    public EntityPropertyChangeEntity()
    {
    }
    
    public EntityPropertyChangeEntity(
        IGuidGenerator guidGenerator,
        Guid entityChangeId,
        EntityPropertyChangeInfo entityChangeInfo,
        Guid? tenantId = null)
    {
        Id = guidGenerator.Create();
        TenantId = tenantId;
        EntityChangeId = entityChangeId;
        NewValue = entityChangeInfo.NewValue.Truncate(EntityPropertyChangeConsts.MaxNewValueLength);
        OriginalValue = entityChangeInfo.OriginalValue.Truncate(EntityPropertyChangeConsts.MaxOriginalValueLength);
        PropertyName =
            entityChangeInfo.PropertyName.TruncateFromBeginning(EntityPropertyChangeConsts.MaxPropertyNameLength);
        PropertyTypeFullName =
            entityChangeInfo.PropertyTypeFullName.TruncateFromBeginning(EntityPropertyChangeConsts
                .MaxPropertyTypeFullNameLength);
    }
    
    public virtual Guid? EntityChangeId { get; protected set; }

    public virtual string? NewValue { get; protected set; }

    public virtual string? OriginalValue { get; protected set; }

    public virtual string? PropertyName { get; protected set; }

    public virtual string? PropertyTypeFullName { get; protected set; }
    
    public virtual Guid? TenantId { get; protected set; }
}