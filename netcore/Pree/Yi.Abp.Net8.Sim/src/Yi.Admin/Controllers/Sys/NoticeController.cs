using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.Admin.Hubs;
using Yi.Sys.Services.Infra;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Admin.Controllers.Sys;

[ApiController]
[Route("api/system/notice")]
public class NoticeController : AbpController
{
    private readonly INoticeService _noticeService;
    private readonly IHubContext<MainHub> _hubContext;

    public NoticeController(INoticeService noticeService,IHubContext<MainHub> hubContext)
    {
        _noticeService = noticeService;
        _hubContext = hubContext;
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
    
    /// <summary>
    ///     发送在线消息
    /// </summary>
    /// <returns></returns>
    [HttpPost("online/{id}")]
    public async Task SendOnlineAsync([FromRoute] Guid id)
    {
        var entity = await _noticeService.GetAsync(id);
        await _hubContext.Clients.All.SendAsync("ReceiveNotice", entity.Type.ToString(), entity.Title, entity.Content);
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