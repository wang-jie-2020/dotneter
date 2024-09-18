using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.System.Notice.Dtos;

namespace Yi.System.Notice.Services;

public interface INoticeService
{
    Task<NoticeDto> GetAsync(Guid id);

    Task<PagedResultDto<NoticeDto>> GetListAsync(NoticeGetListInput input);

    Task<NoticeDto> CreateAsync(NoticeCreateInput input);

    Task<NoticeDto> UpdateAsync(Guid id, NoticeUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(NoticeGetListInput input);

    Task SendOnlineAsync(Guid id);

    Task SendOfflineAsync(Guid id);
}