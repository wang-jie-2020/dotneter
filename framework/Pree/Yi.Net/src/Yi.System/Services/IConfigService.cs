using Volo.Abp.Application.Dtos;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IConfigService
{
    Task<ConfigGetOutputDto> GetAsync(Guid id);

    Task<PagedResultDto<ConfigGetListOutputDto>> GetListAsync(ConfigGetListInputVo input);

    Task<ConfigGetOutputDto> CreateAsync(ConfigCreateInputVo input);

    Task<ConfigGetOutputDto> UpdateAsync(Guid id, ConfigUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}