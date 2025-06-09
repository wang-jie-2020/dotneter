using Volo.Abp.Application.Dtos;
using Yi.System.Monitor.Dtos;

namespace Yi.System.Monitor;

public interface IOperLogService
{
    Task<OperLogDto> GetAsync(long id);

    Task<PagedResultDto<OperLogDto>> GetListAsync(OperLogGetListInput input);

    Task DeleteAsync(IEnumerable<long> id);
}