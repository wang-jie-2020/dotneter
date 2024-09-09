using Yi.Framework.Ddd.Application.Contracts;
using Yi.Infra.Rbac.Dtos.Config;

namespace Yi.Infra.Rbac.IServices;

/// <summary>
///     Config服务抽象
/// </summary>
public interface IConfigService : IYiCrudAppService<ConfigGetOutputDto, ConfigGetListOutputDto, Guid,
    ConfigGetListInputVo, ConfigCreateInputVo, ConfigUpdateInputVo>
{
}