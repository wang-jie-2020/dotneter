using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IDictionaryService
{
    Task<DictionaryDto> GetAsync(long id);

    Task<PagedResult<DictionaryDto>> GetListAsync(DictionaryQuery query);

    Task<DictionaryDto> CreateAsync(DictionaryInput input);

    Task<DictionaryDto> UpdateAsync(long id, DictionaryInput input);

    Task DeleteAsync(IEnumerable<long> id);

    Task<List<DictionaryDto>> GetDicType(string dicType);
}