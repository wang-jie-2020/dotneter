using Volo.Abp.Application.Dtos;
using Yi.Sys.Services.Monitor.Dtos;

namespace Yi.Sys.Services.Monitor;

public interface IOperLogService
{
    Task<OperLogDto> GetAsync(long id);

    Task<PagedResultDto<OperLogDto>> GetListAsync(OperLogGetListInput input);

    Task DeleteAsync(IEnumerable<long> id);
}