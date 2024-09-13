using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.Infra.Rbac.Dtos;

namespace Yi.Infra.Rbac.Services;

public interface IDeptService 
{
    Task<DeptGetOutputDto> GetAsync(Guid id);

    Task<PagedResultDto<DeptGetListOutputDto>> GetListAsync(DeptGetListInput input);

    Task<DeptGetOutputDto> CreateAsync(DeptCreateInput input);

    Task<DeptGetOutputDto> UpdateAsync(Guid id, DeptUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(DeptGetListInput input);
    
    Task<List<Guid>> GetChildListAsync(Guid deptId);

    Task<List<DeptGetListOutputDto>> GetRoleIdAsync(Guid roleId);
}