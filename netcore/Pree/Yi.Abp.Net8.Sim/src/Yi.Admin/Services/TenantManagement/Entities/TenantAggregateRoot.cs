using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Check = Volo.Abp.Check;

namespace Yi.Admin.Services.TenantManagement.Entities;

[SugarTable("Tenant")]
[DefaultTenantTable]
public class TenantAggregateRoot : FullAuditedAggregateRoot<Guid>, IHasEntityVersion
{
    public TenantAggregateRoot()
    {
    }

    protected internal TenantAggregateRoot(Guid id, [NotNull] string name)
        : base(id)
    {
        SetName(name);
    }

    [SugarColumn(IsPrimaryKey = true)] 
    public override Guid Id { get; protected set; }

    public virtual string Name { get; protected set; }

    public string? TenantConnectionString { get; protected set; }

    public DbType DbType { get; protected set; }

    [SugarColumn(IsIgnore = true)]
    public override ExtraPropertyDictionary ExtraProperties
    {
        get => base.ExtraProperties;
        protected set => base.ExtraProperties = value;
    }

    public int EntityVersion { get; protected set; }

    public virtual void SetConnectionString(DbType dbType, string connectionString)
    {
        DbType = dbType;
        TenantConnectionString = connectionString;
    }

    protected internal virtual void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), 64);
    }
}