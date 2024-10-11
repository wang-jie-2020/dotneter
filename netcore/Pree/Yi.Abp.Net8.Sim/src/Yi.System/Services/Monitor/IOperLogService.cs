using Volo.Abp.Application.Dtos;
using Yi.Sys.Services.Monitor.Dtos;

namespace Yi.Sys.Services.Monitor;

public interface IOperLogService
{
    Task<OperLogDto> GetAsync(Guid id);

    Task<PagedResultDto<OperLogDto>> GetListAsync(OperLogGetListInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}