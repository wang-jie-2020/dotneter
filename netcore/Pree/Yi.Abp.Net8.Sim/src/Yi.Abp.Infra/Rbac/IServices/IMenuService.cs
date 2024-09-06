using Yi.Abp.Infra.Rbac.Dtos.Menu;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// Menu服务抽象
    /// </summary>
    public interface IMenuService : IYiCrudAppService<MenuGetOutputDto, MenuGetListOutputDto, Guid, MenuGetListInputVo, MenuCreateInputVo, MenuUpdateInputVo>
    {

    }
}
