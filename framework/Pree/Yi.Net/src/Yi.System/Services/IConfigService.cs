using Yi.System.Services.Dtos;
using ConfigQuery = Yi.System.Services.Dtos.ConfigQuery;

namespace Yi.System.Services;

public interface IConfigService
{
    Task<ConfigDto> GetAsync(long id);

    Task<PagedResult<ConfigDto>> GetListAsync(ConfigQuery query);

    Task<ConfigDto> CreateAsync(ConfigInput input);

    Task<ConfigDto> UpdateAsync(long id, ConfigInput input);

    Task DeleteAsync(IEnumerable<long> id);
}