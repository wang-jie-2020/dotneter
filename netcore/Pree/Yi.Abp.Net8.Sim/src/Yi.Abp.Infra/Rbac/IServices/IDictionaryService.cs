using Yi.Abp.Infra.Rbac.Dtos.Dictionary;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// Dictionary服务抽象
    /// </summary>
    public interface IDictionaryService : IYiCrudAppService<DictionaryGetOutputDto, DictionaryGetListOutputDto, Guid, DictionaryGetListInputVo, DictionaryCreateInputVo, DictionaryUpdateInputVo>
    {

    }
}
