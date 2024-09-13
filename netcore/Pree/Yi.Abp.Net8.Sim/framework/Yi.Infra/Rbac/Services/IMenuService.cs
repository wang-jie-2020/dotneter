using Yi.Infra.Rbac.Dtos;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     Menu服务抽象
/// </summary>
public interface IMenuService : IYiCrudAppService<MenuDto, MenuDto, Guid, MenuGetListInput,
    MenuCreateInput, MenuUpdateInput>
{
}