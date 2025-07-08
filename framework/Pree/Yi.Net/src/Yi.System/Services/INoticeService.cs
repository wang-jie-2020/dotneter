using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface INoticeService
{
    Task<NoticeDto> GetAsync(Guid id);

    Task<PagedResult<NoticeDto>> GetListAsync(NoticeQuery query);

    Task<NoticeDto> CreateAsync(NoticeInput input);

    Task<NoticeDto> UpdateAsync(Guid id, NoticeInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}