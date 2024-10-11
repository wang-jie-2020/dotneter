using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra;

public interface ITenantService 
{
    Task<TenantDto> GetAsync(Guid id);

    Task<PagedResultDto<TenantDto>> GetListAsync(TenantGetListInput input);

    Task<TenantDto> CreateAsync(TenantCreateInput input);

    Task<TenantDto> UpdateAsync(Guid id, TenantUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(TenantGetListInput input);
    
    Task<List<TenantSelectDto>> GetSelectAsync();

    Task InitAsync(Guid id);
}