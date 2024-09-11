using Yi.Infra.Settings.Dtos;

namespace Yi.Infra.Settings.Services;

public interface IDictionaryService : IYiCrudAppService<DictionaryGetOutputDto, DictionaryGetListOutputDto, Guid,
    DictionaryGetListInput, DictionaryCreateInput, DictionaryUpdateInput>
{
}