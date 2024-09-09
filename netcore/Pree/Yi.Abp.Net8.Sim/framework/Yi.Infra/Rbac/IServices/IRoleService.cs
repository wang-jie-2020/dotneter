using Yi.Framework.Ddd.Application;
using Yi.Infra.Rbac.Dtos.Role;

namespace Yi.Infra.Rbac.IServices;

/// <summary>
///     Role服务抽象
/// </summary>
public interface IRoleService : IYiCrudAppService<RoleGetOutputDto, RoleGetListOutputDto, Guid, RoleGetListInputVo,
    RoleCreateInputVo, RoleUpdateInputVo>
{
}