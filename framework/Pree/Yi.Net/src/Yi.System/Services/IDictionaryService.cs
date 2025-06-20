using Yi.Framework.Core;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IDictionaryService
{
    Task<DictionaryDto> GetAsync(Guid id);

    Task<PagedResult<DictionaryDto>> GetListAsync(DictionaryGetListInput input);

    Task<DictionaryDto> CreateAsync(DictionaryCreateInput input);

    Task<DictionaryDto> UpdateAsync(Guid id, DictionaryUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<List<DictionaryDto>> GetDicType(string dicType);
}