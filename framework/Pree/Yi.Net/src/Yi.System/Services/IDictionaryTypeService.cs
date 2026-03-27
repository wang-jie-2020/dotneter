using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IDictionaryTypeService
{
    Task<DictionaryTypeDto> GetAsync(long id);

    Task<PagedResult<DictionaryTypeDto>> GetListAsync(DictionaryTypeQuery query);

    Task<DictionaryTypeDto> CreateAsync(DictionaryTypeInput input);

    Task<DictionaryTypeDto> UpdateAsync(long id, DictionaryTypeInput input);

    Task DeleteAsync(IEnumerable<long> id);
}