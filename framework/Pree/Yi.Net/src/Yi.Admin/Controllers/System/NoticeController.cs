using Microsoft.AspNetCore.Mvc;
using Yi.Framework;
using Yi.Framework.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Web.Controllers.System;

[ApiController]
[Route("api/system/notice")]
public class NoticeController : BaseController
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
    public async Task<PagedResult<NoticeDto>> GetListAsync([FromQuery] NoticeQuery query)
    {
        return await _noticeService.GetListAsync(query);
    }

    [HttpPost]
    public async Task<NoticeDto> CreateAsync([FromBody] NoticeInput input)
    {
        return await _noticeService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<NoticeDto> UpdateAsync(Guid id, [FromBody] NoticeInput input)
    {
        return await _noticeService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _noticeService.DeleteAsync(id);
    }
    
    /// <summary>
    ///     发送在线消息
    /// </summary>
    /// <returns></returns>
    [HttpPost("online/{id}")]
    public async Task SendOnlineAsync([FromRoute] Guid id)
    {
        throw new NotImplementedException();

        //var entity = await _noticeService.GetAsync(id);
        //await _hubContext.Clients.All.SendAsync("ReceiveNotice", entity.Type.ToString(), entity.Title, entity.Content);
    }

    /// <summary>
    ///     发送离线消息
    /// </summary>
    /// <returns></returns>
    [HttpPost("offline/{id}")]
    public async Task SendOfflineAsync([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }
}