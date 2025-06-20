using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Core;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface ITenantService 
{
    Task<TenantDto> GetAsync(Guid id);

    Task<PagedResult<TenantDto>> GetListAsync(TenantGetListInput input);

    Task<TenantDto> CreateAsync(TenantCreateInput input);

    Task<TenantDto> UpdateAsync(Guid id, TenantUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(TenantGetListInput input);
    
    Task<List<TenantSelectDto>> GetSelectAsync();

    Task InitAsync(Guid id);
}