using Volo.Abp.Application.Dtos;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra;

public interface IDeptService 
{
    Task<DeptGetOutputDto> GetAsync(Guid id);

    Task<PagedResultDto<DeptGetListOutputDto>> GetListAsync(DeptGetListInput input);

    Task<DeptGetOutputDto> CreateAsync(DeptCreateInput input);

    Task<DeptGetOutputDto> UpdateAsync(Guid id, DeptUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
    
    Task<List<Guid>> GetChildListAsync(Guid deptId);

    Task<List<DeptGetListOutputDto>> GetRoleIdAsync(Guid roleId);
}