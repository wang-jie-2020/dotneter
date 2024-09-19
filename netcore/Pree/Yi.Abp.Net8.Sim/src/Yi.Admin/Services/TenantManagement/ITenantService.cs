﻿using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.Admin.Services.TenantManagement.Dtos;

namespace Yi.Admin.Services.TenantManagement;

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