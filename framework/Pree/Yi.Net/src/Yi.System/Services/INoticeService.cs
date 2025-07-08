using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface INoticeService
{
    Task<NoticeDto> GetAsync(Guid id);

    Task<PagedResult<NoticeDto>> GetListAsync(NoticeGetListQuery query);

    Task<NoticeDto> CreateAsync(NoticeCreateInput input);

    Task<NoticeDto> UpdateAsync(Guid id, NoticeUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}