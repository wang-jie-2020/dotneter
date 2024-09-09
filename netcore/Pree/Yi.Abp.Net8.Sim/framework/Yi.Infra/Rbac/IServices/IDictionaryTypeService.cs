using Yi.Framework.Ddd.Application;
using Yi.Infra.Rbac.Dtos.DictionaryType;

namespace Yi.Infra.Rbac.IServices;

/// <summary>
///     DictionaryType服务抽象
/// </summary>
public interface IDictionaryTypeService : IYiCrudAppService<DictionaryTypeGetOutputDto, DictionaryTypeGetListOutputDto,
    Guid, DictionaryTypeGetListInputVo, DictionaryTypeCreateInputVo, DictionaryTypeUpdateInputVo>
{
}