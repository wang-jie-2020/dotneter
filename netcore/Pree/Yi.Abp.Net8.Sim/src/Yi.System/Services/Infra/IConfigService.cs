using Volo.Abp.Application.Dtos;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra;

public interface IConfigService
{
    Task<ConfigGetOutputDto> GetAsync(Guid id);

    Task<PagedResultDto<ConfigGetListOutputDto>> GetListAsync(ConfigGetListInputVo input);

    Task<ConfigGetOutputDto> CreateAsync(ConfigCreateInputVo input);

    Task<ConfigGetOutputDto> UpdateAsync(Guid id, ConfigUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}