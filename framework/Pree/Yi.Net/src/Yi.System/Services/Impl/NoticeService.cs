using Yi.Framework.Abstractions;
using Yi.System.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class NoticeService : BaseService, INoticeService
{
    private readonly ISqlSugarRepository<NoticeEntity> _repository;

    public NoticeService(ISqlSugarRepository<NoticeEntity> repository)
    {
        _repository = repository;
    }

    public async Task<NoticeDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<NoticeDto>();
    }

    public async Task<PagedResult<NoticeDto>> GetListAsync(NoticeGetListQuery query)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable().WhereIF(query.Type is not null, x => x.Type == query.Type)
            .WhereIF(!string.IsNullOrEmpty(query.Title), x => x.Title!.Contains(query.Title!))
            .WhereIF(query.StartTime is not null && query.EndTime is not null, x => x.CreationTime >= query.StartTime && x.CreationTime <= query.EndTime)
            .ToPageListAsync(query.PageNum, query.PageSize, total);

        return new PagedResult<NoticeDto>(total, entities.Adapt<List<NoticeDto>>());
    }

    public async Task<NoticeDto> CreateAsync(NoticeCreateInput input)
    {
        var entity = input.Adapt<NoticeEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<NoticeDto>();
    }

    public async Task<NoticeDto> UpdateAsync(Guid id, NoticeUpdateInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<NoticeDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
}