using Volo.Abp.Application.Dtos;
using Yi.System.Services.Sys.Dtos;

namespace Yi.System.Services.Sys;

public interface IDictionaryService
{
    Task<DictionaryDto> GetAsync(Guid id);

    Task<PagedResultDto<DictionaryDto>> GetListAsync(DictionaryGetListInput input);

    Task<DictionaryDto> CreateAsync(DictionaryCreateInput input);

    Task<DictionaryDto> UpdateAsync(Guid id, DictionaryUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<List<DictionaryDto>> GetDicType(string dicType);
}