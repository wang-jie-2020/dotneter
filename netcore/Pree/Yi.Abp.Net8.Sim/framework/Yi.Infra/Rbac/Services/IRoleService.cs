using Yi.Infra.Rbac.Dtos;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     Role服务抽象
/// </summary>
public interface IRoleService : IYiCrudAppService<RoleGetOutputDto, RoleGetListOutputDto, Guid, RoleGetListInput,
    RoleCreateInput, RoleUpdateInput>
{
}