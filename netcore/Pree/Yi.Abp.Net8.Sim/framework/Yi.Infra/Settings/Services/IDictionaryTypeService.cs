using Volo.Abp.Application.Dtos;
using Yi.Infra.Settings.Dtos;

namespace Yi.Infra.Settings.Services;

public interface IDictionaryTypeService
{
    Task<DictionaryTypeDto> GetAsync(Guid id);

    Task<PagedResultDto<DictionaryTypeDto>> GetListAsync(DictionaryTypeGetListInput input);

    Task<DictionaryTypeDto> CreateAsync(DictionaryTypeCreateInput input);

    Task<DictionaryTypeDto> UpdateAsync(Guid id, DictionaryTypeUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}