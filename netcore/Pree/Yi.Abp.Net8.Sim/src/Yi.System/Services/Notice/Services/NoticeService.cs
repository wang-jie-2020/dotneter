using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.System.Hubs;
using Yi.System.Services.Notice.Dtos;
using Yi.System.Services.Notice.Entities;

namespace Yi.System.Services.Notice.Services;

[RemoteService(false)]
public class NoticeService : ApplicationService, INoticeService
{
    private readonly ISqlSugarRepository<NoticeAggregateRoot, Guid> _repository;
    private readonly IHubContext<MainHub> _hubContext;

    public NoticeService(ISqlSugarRepository<NoticeAggregateRoot, Guid> repository, IHubContext<MainHub> hubContext)
    {
        _hubContext = hubContext;
        _repository = repository;
    }

    public async Task<NoticeDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<NoticeDto>();
    }

    public async Task<PagedResultDto<NoticeDto>> GetListAsync(NoticeGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable.WhereIF(input.Type is not null, x => x.Type == input.Type)
            .WhereIF(!string.IsNullOrEmpty(input.Title), x => x.Title!.Contains(input.Title!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<NoticeDto>(total, entities.Adapt<List<NoticeDto>>());
    }

    public async Task<NoticeDto> CreateAsync(NoticeCreateInput input)
    {
        var entity = input.Adapt<NoticeAggregateRoot>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<NoticeDto>();
    }

    public async Task<NoticeDto> UpdateAsync(Guid id, NoticeUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<NoticeDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(NoticeGetListInput input)
    {
        if (input is IPagedResultRequest paged)
        {
            paged.SkipCount = 0;
            paged.MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount;
        }

        var output = await GetListAsync(input);
        return new PhysicalFileResult(ExporterHelper.ExportExcel(output.Items), "application/vnd.ms-excel");
    }

    /// <summary>
    ///     发送在线消息
    /// </summary>
    /// <returns></returns>
    public async Task SendOnlineAsync(Guid id)
    {
        var entity = await _repository.DbQueryable.FirstAsync(x => x.Id == id);
        await _hubContext.Clients.All.SendAsync("ReceiveNotice", entity.Type.ToString(), entity.Title, entity.Content);
    }

    /// <summary>
    ///     发送离线消息
    /// </summary>
    /// <returns></returns>
    public async Task SendOfflineAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}