using Yi.Infra.Settings.Dtos;

namespace Yi.Infra.Settings.Services;

public interface IConfigService : IYiCrudAppService<ConfigGetOutputDto, ConfigGetListOutputDto, Guid,
    ConfigGetListInputVo, ConfigCreateInputVo, ConfigUpdateInputVo>
{
}