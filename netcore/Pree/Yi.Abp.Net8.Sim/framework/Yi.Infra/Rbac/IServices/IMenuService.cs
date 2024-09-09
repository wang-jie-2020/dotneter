using Yi.Framework.Ddd.Application;
using Yi.Infra.Rbac.Dtos.Menu;

namespace Yi.Infra.Rbac.IServices;

/// <summary>
///     Menu服务抽象
/// </summary>
public interface IMenuService : IYiCrudAppService<MenuGetOutputDto, MenuGetListOutputDto, Guid, MenuGetListInputVo,
    MenuCreateInputVo, MenuUpdateInputVo>
{
}