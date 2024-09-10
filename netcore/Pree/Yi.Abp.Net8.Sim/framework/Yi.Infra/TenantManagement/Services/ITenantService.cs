using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.Infra.TenantManagement.Dtos;

namespace Yi.Infra.TenantManagement.Services;

public interface ITenantService 
{
    Task<TenantGetOutputDto> GetAsync(Guid id);

    Task<PagedResultDto<TenantGetListOutputDto>> GetListAsync(TenantGetListInput input);

    Task<TenantGetOutputDto> CreateAsync(TenantCreateInput input);

    Task<TenantGetOutputDto> UpdateAsync(Guid id, TenantUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<List<TenantSelectOutputDto>> GetSelectAsync();

    Task InitAsync(Guid id);

    Task<IActionResult> GetExportExcelAsync(TenantGetListInput input);
}