using JetBrains.Annotations;
using SqlSugar;
using Yi.Framework.SqlSugarCore;
using Check = Volo.Abp.Check;

namespace Yi.Framework.Core.Entities;

[SugarTable("Sys_Tenant")]
[DefaultTenantTable]
public class TenantEntity : BizEntity<long>
{
    public TenantEntity()
    {
    }

    protected internal TenantEntity(long id, [NotNull] string name) : base(id)
    {
        SetName(name);
    }
    
    public virtual string Name { get; protected set; }

    public string? TenantConnectionString { get; protected set; }

    public DbType DbType { get; protected set; }
    
    protected internal virtual void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), 64);
    }
}