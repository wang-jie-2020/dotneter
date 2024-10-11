using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.Sys.Domains.Infra.Entities;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra.Impl;

public class NoticeService : ApplicationService, INoticeService
{
    private readonly ISqlSugarRepository<NoticeEntity, Guid> _repository;

    public NoticeService(ISqlSugarRepository<NoticeEntity, Guid> repository)
    {
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
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.PageNum, input.PageSize, total);

        return new PagedResultDto<NoticeDto>(total, entities.Adapt<List<NoticeDto>>());
    }

    public async Task<NoticeDto> CreateAsync(NoticeCreateInput input)
    {
        var entity = input.Adapt<NoticeEntity>();
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
}