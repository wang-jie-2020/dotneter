using Yi.Infra.Rbac.Dtos;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     Role服务抽象
/// </summary>
public interface IRoleService : IYiCrudAppService<RoleDto, RoleDto, Guid, RoleGetListInput,
    RoleCreateInput, RoleUpdateInput>
{
}