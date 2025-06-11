using Yi.AspNetCore.Core;
using Yi.System.Monitor.Dtos;

namespace Yi.System.Monitor;

public interface IOperLogService
{
    Task<OperLogDto> GetAsync(long id);

    Task<PagedResult<OperLogDto>> GetListAsync(OperLogGetListInput input);

    Task DeleteAsync(IEnumerable<long> id);
}