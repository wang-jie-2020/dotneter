using Volo.Abp.Application.Dtos;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra;

public interface IDictionaryService
{
    Task<DictionaryDto> GetAsync(Guid id);

    Task<PagedResultDto<DictionaryDto>> GetListAsync(DictionaryGetListInput input);

    Task<DictionaryDto> CreateAsync(DictionaryCreateInput input);

    Task<DictionaryDto> UpdateAsync(Guid id, DictionaryUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<List<DictionaryDto>> GetDicType(string dicType);
}