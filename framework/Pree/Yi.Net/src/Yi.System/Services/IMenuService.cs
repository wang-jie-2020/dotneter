using Yi.Framework.Core;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IMenuService
{
    Task<MenuDto> GetAsync(Guid id);

    Task<PagedResult<MenuDto>> GetListAsync(MenuGetListInput input);

    Task<MenuDto> CreateAsync(MenuCreateInput input);

    Task<MenuDto> UpdateAsync(Guid id, MenuUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
    
    Task<List<MenuDto>> GetListRoleIdAsync(Guid roleId);
}