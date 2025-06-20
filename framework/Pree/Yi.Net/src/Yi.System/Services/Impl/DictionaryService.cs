using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Core;
using Yi.Framework.Core.Abstractions;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Domains.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class DictionaryService : BaseService, IDictionaryService
{
    private readonly ISqlSugarRepository<DictionaryEntity, Guid> _repository;

    public DictionaryService(ISqlSugarRepository<DictionaryEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<DictionaryDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<DictionaryDto>();
    }

    public async Task<PagedResult<DictionaryDto>> GetListAsync(DictionaryGetListInput input)
    {
        RefAsync<int> total = 0;
        var entities = await _repository.DbQueryable
            .WhereIF(input.DictType is not null, x => x.DictType == input.DictType)
            .WhereIF(input.DictLabel is not null, x => x.DictLabel!.Contains(input.DictLabel!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .ToPageListAsync(input.PageNum, input.PageSize, total);
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
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<DictionaryDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
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