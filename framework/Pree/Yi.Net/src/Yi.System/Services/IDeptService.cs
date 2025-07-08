using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IDeptService 
{
    Task<DeptDto> GetAsync(Guid id);

    Task<PagedResult<DeptDto>> GetListAsync(DeptQuery query);

    Task<DeptDto> CreateAsync(DeptInput input);

    Task<DeptDto> UpdateAsync(Guid id, DeptInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
    
    Task<List<Guid>> GetChildListAsync(Guid deptId);

    Task<List<DeptDto>> GetRoleIdAsync(Guid roleId);
}