using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.System.Services.System.Dtos;

namespace Yi.System.Services.System;

public interface IConfigService
{
    Task<ConfigGetOutputDto> GetAsync(Guid id);

    Task<PagedResultDto<ConfigGetListOutputDto>> GetListAsync(ConfigGetListInputVo input);

    Task<ConfigGetOutputDto> CreateAsync(ConfigCreateInputVo input);

    Task<ConfigGetOutputDto> UpdateAsync(Guid id, ConfigUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(ConfigGetListInputVo input);
}