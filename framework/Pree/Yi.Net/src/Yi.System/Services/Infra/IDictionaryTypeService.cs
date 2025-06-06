using Volo.Abp.Application.Dtos;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra;

public interface IDictionaryTypeService
{
    Task<DictionaryTypeDto> GetAsync(Guid id);

    Task<PagedResultDto<DictionaryTypeDto>> GetListAsync(DictionaryTypeGetListInput input);

    Task<DictionaryTypeDto> CreateAsync(DictionaryTypeCreateInput input);

    Task<DictionaryTypeDto> UpdateAsync(Guid id, DictionaryTypeUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}