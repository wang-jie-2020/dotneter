using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface ILanguageService
{
    Task<LanguageDto> GetAsync(long id);

    Task<PagedResult<LanguageDto>> GetListAsync(LanguageQuery query);

    Task<LanguageDto> CreateAsync(LanguageInput input);

    Task<LanguageDto> UpdateAsync(long id, LanguageInput input);

    Task DeleteAsync(IEnumerable<long> id);
    
    Task<Dictionary<string, string>> GetMessagesAsync(string culture);
}
