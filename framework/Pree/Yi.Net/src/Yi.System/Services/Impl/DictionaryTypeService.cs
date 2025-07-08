using Yi.Framework.Abstractions;
using Yi.System.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class DictionaryTypeService : BaseService, IDictionaryTypeService
{
    private readonly ISqlSugarRepository<DictionaryTypeEntity> _repository;

    public DictionaryTypeService(ISqlSugarRepository<DictionaryTypeEntity> repository)
    {
        _repository = repository;
    }

    public async Task<DictionaryTypeDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<DictionaryTypeDto>();
    }

    public async Task<PagedResult<DictionaryTypeDto>> GetListAsync(DictionaryTypeGetListQuery query)
    {
        RefAsync<int> total = 0;
        var entities = await _repository.AsQueryable()
            .WhereIF(query.DictName is not null, x => x.DictName.Contains(query.DictName!))
            .WhereIF(query.DictType is not null, x => x.DictType!.Contains(query.DictType!))
            .WhereIF(query.State is not null, x => x.State == query.State)
            .WhereIF(query.StartTime is not null && query.EndTime is not null, x => x.CreationTime >= query.StartTime && x.CreationTime <= query.EndTime)
            .ToPageListAsync(query.PageNum, query.PageSize, total);

        return new PagedResult<DictionaryTypeDto>
        {
            TotalCount = total,
            Items = entities.Adapt<List<DictionaryTypeDto>>()
        };
    }

    public async Task<DictionaryTypeDto> CreateAsync(DictionaryTypeCreateInput input)
    {
        var entity = input.Adapt<DictionaryTypeEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<DictionaryTypeDto>();
    }

    public async Task<DictionaryTypeDto> UpdateAsync(Guid id, DictionaryTypeUpdateInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<DictionaryTypeDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
}