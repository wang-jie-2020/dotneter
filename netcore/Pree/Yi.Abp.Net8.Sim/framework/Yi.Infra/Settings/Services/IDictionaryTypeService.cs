using Yi.Infra.Settings.Dtos;

namespace Yi.Infra.Settings.Services;

public interface IDictionaryTypeService : IYiCrudAppService<DictionaryTypeGetOutputDto, DictionaryTypeGetListOutputDto,
    Guid, DictionaryTypeGetListInputVo, DictionaryTypeCreateInputVo, DictionaryTypeUpdateInputVo>
{
}