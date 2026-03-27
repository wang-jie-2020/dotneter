using Microsoft.AspNetCore.Mvc;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface ITenantService 
{
    Task<TenantDto> GetAsync(long id);

    Task<PagedResult<TenantDto>> GetListAsync(TenantQuery query);

    Task<TenantDto> CreateAsync(TenantInput input);

    Task<TenantDto> UpdateAsync(long id, TenantInput input);

    Task DeleteAsync(IEnumerable<long> id);

    Task<IActionResult> GetExportExcelAsync(TenantQuery query);
    
    Task<List<TenantSelectDto>> GetSelectAsync();

    Task InitAsync(long id);
}