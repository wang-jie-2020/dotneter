using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.Admin.Services.OperationLogging.Dtos;

namespace Yi.Admin.Services.OperationLogging;

public interface IOperationLogService
{
    Task<OperationLogDto> GetAsync(Guid id);

    Task<PagedResultDto<OperationLogDto>> GetListAsync(OperationLogGetListInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(OperationLogGetListInput input);
}