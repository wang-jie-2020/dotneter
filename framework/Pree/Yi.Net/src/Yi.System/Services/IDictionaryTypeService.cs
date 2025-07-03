using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IDictionaryTypeService
{
    Task<DictionaryTypeDto> GetAsync(Guid id);

    Task<PagedResult<DictionaryTypeDto>> GetListAsync(DictionaryTypeGetListInput input);

    Task<DictionaryTypeDto> CreateAsync(DictionaryTypeCreateInput input);

    Task<DictionaryTypeDto> UpdateAsync(Guid id, DictionaryTypeUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}