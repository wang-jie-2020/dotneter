using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IMenuService
{
    Task<MenuDto> GetAsync(long id);

    Task<PagedResult<MenuDto>> GetListAsync(MenuQuery query);

    Task<MenuDto> CreateAsync(MenuInput input);

    Task<MenuDto> UpdateAsync(long id, MenuInput input);

    Task DeleteAsync(IEnumerable<long> id);
    
    Task<List<MenuDto>> GetListRoleIdAsync(long roleId);
}