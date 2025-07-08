using Microsoft.AspNetCore.Mvc;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface ITenantService 
{
    Task<TenantDto> GetAsync(Guid id);

    Task<PagedResult<TenantDto>> GetListAsync(TenantQuery query);

    Task<TenantDto> CreateAsync(TenantInput input);

    Task<TenantDto> UpdateAsync(Guid id, TenantInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(TenantQuery query);
    
    Task<List<TenantSelectDto>> GetSelectAsync();

    Task InitAsync(Guid id);
}