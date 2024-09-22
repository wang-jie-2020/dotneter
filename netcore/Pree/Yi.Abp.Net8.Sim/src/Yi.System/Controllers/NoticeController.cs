using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.System.Services.Sys;
using Yi.System.Services.Sys.Dtos;

namespace Yi.System.Controllers;

[ApiController]
[Route("api/notice")]
public class NoticeController : AbpController
{
    private readonly INoticeService _noticeService;

    public NoticeController(INoticeService noticeService)
    {
        _noticeService = noticeService;
    }

    [HttpGet("{id}")]
    public async Task<NoticeDto> GetAsync(Guid id)
    {
        return await _noticeService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<NoticeDto>> GetListAsync([FromQuery] NoticeGetListInput input)
    {
        return await _noticeService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<NoticeDto> CreateAsync([FromBody] NoticeCreateInput input)
    {
        return await _noticeService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<NoticeDto> UpdateAsync(Guid id, [FromBody] NoticeUpdateInput input)
    {
        return await _noticeService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _noticeService.DeleteAsync(id);
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] NoticeGetListInput input)
    {
        return await _noticeService.GetExportExcelAsync(input);
    }

    /// <summary>
    ///     发送在线消息
    /// </summary>
    /// <returns></returns>
    [HttpPost("online/{id}")]
    public async Task SendOnlineAsync([FromRoute] Guid id)
    {
        await _noticeService.SendOnlineAsync(id);
    }

    /// <summary>
    ///     发送离线消息
    /// </summary>
    /// <returns></returns>
    [HttpPost("offline/{id}")]
    public async Task SendOfflineAsync([FromRoute] Guid id)
    {
        await _noticeService.SendOfflineAsync(id);
    }
}