using Yi.Framework;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IDeptService 
{
    Task<DeptGetOutputDto> GetAsync(Guid id);

    Task<PagedResult<DeptGetListOutputDto>> GetListAsync(DeptGetListInput input);

    Task<DeptGetOutputDto> CreateAsync(DeptCreateInput input);

    Task<DeptGetOutputDto> UpdateAsync(Guid id, DeptUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
    
    Task<List<Guid>> GetChildListAsync(Guid deptId);

    Task<List<DeptGetListOutputDto>> GetRoleIdAsync(Guid roleId);
}