using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.System.Services.Monitor.Dtos;

namespace Yi.System.Services.Monitor;

public interface IOperLogService
{
    Task<OperLogDto> GetAsync(Guid id);

    Task<PagedResultDto<OperLogDto>> GetListAsync(OperLogGetListInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(OperLogGetListInput input);
}