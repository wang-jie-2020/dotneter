using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Abstractions;
using Yi.System.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class DictionaryService : BaseService, IDictionaryService
{
    private readonly ISqlSugarRepository<DictionaryEntity> _repository;

    public DictionaryService(ISqlSugarRepository<DictionaryEntity> repository)
    {
        _repository = repository;
    }

    public async Task<DictionaryDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<DictionaryDto>();
    }

    public async Task<PagedResult<DictionaryDto>> GetListAsync(DictionaryGetListQuery query)
    {
        RefAsync<int> total = 0;
        var entities = await _repository.AsQueryable()
            .WhereIF(query.DictType is not null, x => x.DictType == query.DictType)
            .WhereIF(query.DictLabel is not null, x => x.DictLabel!.Contains(query.DictLabel!))
            .WhereIF(query.State is not null, x => x.State == query.State)
            .ToPageListAsync(query.PageNum, query.PageSize, total);
        return new PagedResult<DictionaryDto>
        {
            TotalCount = total,
            Items = entities.Adapt<List<DictionaryDto>>()
        };
    }

    public async Task<DictionaryDto> CreateAsync(DictionaryCreateInput input)
    {
        var entity = input.Adapt<DictionaryEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<DictionaryDto>();
    }

    public async Task<DictionaryDto> UpdateAsync(Guid id, DictionaryUpdateInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<DictionaryDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }

    /// <summary>
    ///     根据字典类型获取字典列表
    /// </summary>
    /// <param name="dicType"></param>
    /// <returns></returns>
    public async Task<List<DictionaryDto>> GetDicType([FromRoute] string dicType)
    {
        var entities = await _repository.GetListAsync(u => u.DictType == dicType && u.State == true);
        var result = entities.Adapt<List<DictionaryDto>>();
        return result;
    }
}