using Yi.Abp.Infra.Rbac.Dtos.DictionaryType;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// DictionaryType服务抽象
    /// </summary>
    public interface IDictionaryTypeService : IYiCrudAppService<DictionaryTypeGetOutputDto, DictionaryTypeGetListOutputDto, Guid, DictionaryTypeGetListInputVo, DictionaryTypeCreateInputVo, DictionaryTypeUpdateInputVo>
    {

    }
}
