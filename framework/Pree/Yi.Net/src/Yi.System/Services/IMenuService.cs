using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IMenuService
{
    Task<MenuDto> GetAsync(Guid id);

    Task<PagedResult<MenuDto>> GetListAsync(MenuQuery query);

    Task<MenuDto> CreateAsync(MenuInput input);

    Task<MenuDto> UpdateAsync(Guid id, MenuInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
    
    Task<List<MenuDto>> GetListRoleIdAsync(Guid roleId);
}