using Volo.Abp.Application.Dtos;
using Yi.System.Services.System.Dtos;

namespace Yi.System.Services.System;

public interface IDictionaryTypeService
{
    Task<DictionaryTypeDto> GetAsync(Guid id);

    Task<PagedResultDto<DictionaryTypeDto>> GetListAsync(DictionaryTypeGetListInput input);

    Task<DictionaryTypeDto> CreateAsync(DictionaryTypeCreateInput input);

    Task<DictionaryTypeDto> UpdateAsync(Guid id, DictionaryTypeUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}