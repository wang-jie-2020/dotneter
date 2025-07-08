using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IDictionaryService
{
    Task<DictionaryDto> GetAsync(Guid id);

    Task<PagedResult<DictionaryDto>> GetListAsync(DictionaryQuery query);

    Task<DictionaryDto> CreateAsync(DictionaryInput input);

    Task<DictionaryDto> UpdateAsync(Guid id, DictionaryInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<List<DictionaryDto>> GetDicType(string dicType);
}