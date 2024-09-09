using SqlSugar;
using Volo.Abp.Domain.Entities;

namespace Yi.Infra.Rbac.Entities;

public class StudentEntity : Entity<Guid>
{
    [SugarColumn(IsPrimaryKey = true)] public override Guid Id { get; protected set; }

    public string Name { get; set; }
}