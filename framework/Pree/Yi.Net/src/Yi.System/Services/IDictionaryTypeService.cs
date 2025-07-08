using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IDictionaryTypeService
{
    Task<DictionaryTypeDto> GetAsync(Guid id);

    Task<PagedResult<DictionaryTypeDto>> GetListAsync(DictionaryTypeQuery query);

    Task<DictionaryTypeDto> CreateAsync(DictionaryTypeInput input);

    Task<DictionaryTypeDto> UpdateAsync(Guid id, DictionaryTypeInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}