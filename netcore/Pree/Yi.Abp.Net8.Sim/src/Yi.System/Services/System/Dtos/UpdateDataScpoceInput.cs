using Yi.System.Domains.System;

namespace Yi.System.Services.System.Dtos;

public class UpdateDataScopeInput
{
    public Guid RoleId { get; set; }

    public List<Guid>? DeptIds { get; set; }

    public DataScopeEnum DataScope { get; set; }
}