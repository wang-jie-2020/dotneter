using Yi.Abp.Infra.Rbac.Dtos.Config;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// Config服务抽象
    /// </summary>
    public interface IConfigService : IYiCrudAppService<ConfigGetOutputDto, ConfigGetListOutputDto, Guid, ConfigGetListInputVo, ConfigCreateInputVo, ConfigUpdateInputVo>
    {

    }
}
