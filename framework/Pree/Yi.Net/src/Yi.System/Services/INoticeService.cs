using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface INoticeService
{
    Task<NoticeDto> GetAsync(long id);

    Task<PagedResult<NoticeDto>> GetListAsync(NoticeQuery query);

    Task<NoticeDto> CreateAsync(NoticeInput input);

    Task<NoticeDto> UpdateAsync(long id, NoticeInput input);

    Task DeleteAsync(IEnumerable<long> id);
}