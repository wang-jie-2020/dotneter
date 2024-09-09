using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Framework.Ddd.Application;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Dtos.Notice;
using Yi.Infra.Rbac.Entities;
using Yi.Infra.Rbac.IServices;
using Yi.Infra.Rbac.SignalRHubs;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     Notice服务实现
/// </summary>
public class NoticeService : YiCrudAppService<NoticeAggregateRoot, NoticeGetOutputDto, NoticeGetListOutputDto, Guid,
        NoticeGetListInput, NoticeCreateInput, NoticeUpdateInput>,
    INoticeService
{
    private readonly IHubContext<NoticeHub> _hubContext;
    private readonly ISqlSugarRepository<NoticeAggregateRoot, Guid> _repository;

    public NoticeService(ISqlSugarRepository<NoticeAggregateRoot, Guid> repository, IHubContext<NoticeHub> hubContext) :
        base(repository)
    {
        _hubContext = hubContext;
        _repository = repository;
    }

    /// <summary>
    ///     多查
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public override async Task<PagedResultDto<NoticeGetListOutputDto>> GetListAsync(NoticeGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository._DbQueryable.WhereIF(input.Type is not null, x => x.Type == input.Type)
            .WhereIF(!string.IsNullOrEmpty(input.Title), x => x.Title!.Contains(input.Title!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
        return new PagedResultDto<NoticeGetListOutputDto>(total, await MapToGetListOutputDtosAsync(entities));
    }


    /// <summary>
    ///     发送在线消息
    /// </summary>
    /// <returns></returns>
    [HttpPost("notice/online/{id}")]
    public async Task SendOnlineAsync([FromRoute] Guid id)
    {
        var entity = await _repository._DbQueryable.FirstAsync(x => x.Id == id);
        await _hubContext.Clients.All.SendAsync("ReceiveNotice", entity.Type.ToString(), entity.Title, entity.Content);
    }

    /// <summary>
    ///     发送离线消息
    /// </summary>
    /// <returns></returns>
    [HttpPost("notice/offline/{id}")]
    public async Task SendOfflineAsync([FromRoute] Guid id)
    {
        //先发送一个在线
        await SendOnlineAsync(id);

        //然后将所有用户和通知id进行保留记录，判断是否已读还是未读
        //在首次请求返回全部未读的通知给前端即可
    }
}