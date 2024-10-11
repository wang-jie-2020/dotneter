using Yi.System.Domains.Sys;

namespace Yi.System.Services.Sys.Dtos;

public class UpdateDataScopeInput
{
    public Guid RoleId { get; set; }

    public List<Guid>? DeptIds { get; set; }

    public DataScopeEnum DataScope { get; set; }
}