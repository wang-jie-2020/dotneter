using Yi.AspNetCore.Core;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IConfigService
{
    Task<ConfigGetOutputDto> GetAsync(Guid id);

    Task<PagedResult<ConfigGetListOutputDto>> GetListAsync(ConfigGetListInputVo input);

    Task<ConfigGetOutputDto> CreateAsync(ConfigCreateInputVo input);

    Task<ConfigGetOutputDto> UpdateAsync(Guid id, ConfigUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}