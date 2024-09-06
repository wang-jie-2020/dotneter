using Yi.Abp.Infra.Rbac.Dtos.Role;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// Role服务抽象
    /// </summary>
    public interface IRoleService : IYiCrudAppService<RoleGetOutputDto, RoleGetListOutputDto, Guid, RoleGetListInputVo, RoleCreateInputVo, RoleUpdateInputVo>
    {

    }
}
