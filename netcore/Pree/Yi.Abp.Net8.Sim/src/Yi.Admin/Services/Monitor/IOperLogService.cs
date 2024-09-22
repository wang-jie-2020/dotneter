using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.Admin.Services.Monitor.Dtos;

namespace Yi.Admin.Services.Monitor;

public interface IOperLogService
{
    Task<OperLogDto> GetAsync(Guid id);

    Task<PagedResultDto<OperLogDto>> GetListAsync(OperLogGetListInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(OperLogGetListInput input);
}