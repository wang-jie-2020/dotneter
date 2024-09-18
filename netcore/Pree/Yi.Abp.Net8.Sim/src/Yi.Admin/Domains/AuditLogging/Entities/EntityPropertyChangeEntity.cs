using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Yi.Admin.Domains.AuditLogging.Consts;

namespace Yi.Admin.Domains.AuditLogging.Entities;

[SugarTable("EntityPropertyChange")]
[SugarIndex($"index_{nameof(EntityChangeId)}", nameof(EntityChangeId), OrderByType.Asc)]
public class EntityPropertyChangeEntity : Entity<Guid>, IMultiTenant
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

    [SugarColumn(ColumnName = "Id", IsPrimaryKey = true)]
    public override Guid Id { get; protected set; }

    public virtual Guid? EntityChangeId { get; protected set; }

    public virtual string? NewValue { get; protected set; }

    public virtual string? OriginalValue { get; protected set; }

    public virtual string? PropertyName { get; protected set; }

    public virtual string? PropertyTypeFullName { get; protected set; }
    
    public virtual Guid? TenantId { get; protected set; }
}