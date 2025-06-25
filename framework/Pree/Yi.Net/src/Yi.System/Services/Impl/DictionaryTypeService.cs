using Yi.Framework;
using Yi.Framework.Abstractions;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Domains.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class DictionaryTypeService : BaseService, IDictionaryTypeService
{
    private readonly ISqlSugarRepository<DictionaryTypeEntity, Guid> _repository;

    public DictionaryTypeService(ISqlSugarRepository<DictionaryTypeEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<DictionaryTypeDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<DictionaryTypeDto>();
    }

    public async Task<PagedResult<DictionaryTypeDto>> GetListAsync(DictionaryTypeGetListInput input)
    {
        RefAsync<int> total = 0;
        var entities = await _repository.AsQueryable()
            .WhereIF(input.DictName is not null, x => x.DictName.Contains(input.DictName!))
            .WhereIF(input.DictType is not null, x => x.DictType!.Contains(input.DictType!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.PageNum, input.PageSize, total);

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
        await _repository.DeleteManyAsync(id);
    }
}