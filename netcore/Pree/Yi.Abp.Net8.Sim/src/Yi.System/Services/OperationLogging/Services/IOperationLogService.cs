using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.System.Services.OperationLogging.Dtos;

namespace Yi.System.Services.OperationLogging.Services;

public interface IOperationLogService
{
    Task<OperationLogDto> GetAsync(Guid id);

    Task<PagedResultDto<OperationLogDto>> GetListAsync(OperationLogGetListInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(OperationLogGetListInput input);
}