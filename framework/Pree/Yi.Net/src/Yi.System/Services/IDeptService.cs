using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IDeptService 
{
    Task<DeptDto> GetAsync(long id);

    Task<PagedResult<DeptDto>> GetListAsync(DeptQuery query);

    Task<DeptDto> CreateAsync(DeptInput input);

    Task<DeptDto> UpdateAsync(long id, DeptInput input);

    Task DeleteAsync(IEnumerable<long> id);
    
    Task<List<long>> GetChildListAsync(long deptId);

    Task<List<DeptDto>> GetRoleIdAsync(long roleId);
}