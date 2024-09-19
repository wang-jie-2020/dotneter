using Yi.System.Domains.Rbac;

namespace Yi.System.Services.Rbac.Dtos;

public class UpdateDataScopeInput
{
    public Guid RoleId { get; set; }

    public List<Guid>? DeptIds { get; set; }

    public DataScopeEnum DataScope { get; set; }
}