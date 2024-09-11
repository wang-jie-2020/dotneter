using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.Infra.Settings.Dtos;

namespace Yi.Infra.Settings.Services;

public interface IConfigService
{
    Task<ConfigGetOutputDto> GetAsync(Guid id);

    Task<PagedResultDto<ConfigGetListOutputDto>> GetListAsync(ConfigGetListInputVo input);

    Task<ConfigGetOutputDto> CreateAsync(ConfigCreateInputVo input);

    Task<ConfigGetOutputDto> UpdateAsync(Guid id, ConfigUpdateInputVo input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(ConfigGetListInputVo input);
}