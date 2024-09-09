using Yi.Framework.Ddd.Application.Contracts;
using Yi.Infra.Rbac.Dtos.Dictionary;

namespace Yi.Infra.Rbac.IServices;

/// <summary>
///     Dictionary服务抽象
/// </summary>
public interface IDictionaryService : IYiCrudAppService<DictionaryGetOutputDto, DictionaryGetListOutputDto, Guid,
    DictionaryGetListInputVo, DictionaryCreateInputVo, DictionaryUpdateInputVo>
{
}