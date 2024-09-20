using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.System.Domains.Sys.Entities;
using Yi.System.Services.Sys.Dtos;

namespace Yi.System.Services.Sys;

public class DictionaryTypeService : ApplicationService, IDictionaryTypeService
{
    private readonly ISqlSugarRepository<DictionaryTypeEntity, Guid> _repository;

    public DictionaryTypeService(ISqlSugarRepository<DictionaryTypeEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<DictionaryTypeDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<DictionaryTypeDto>();
    }

    public async Task<PagedResultDto<DictionaryTypeDto>> GetListAsync(DictionaryTypeGetListInput input)
    {
        RefAsync<int> total = 0;
        var entities = await _repository.DbQueryable
            .WhereIF(input.DictName is not null, x => x.DictName.Contains(input.DictName!))
            .WhereIF(input.DictType is not null, x => x.DictType!.Contains(input.DictType!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<DictionaryTypeDto>
        {
            TotalCount = total,
            Items = entities.Adapt<List<DictionaryTypeDto>>()
        };
    }

    public async Task<DictionaryTypeDto> CreateAsync(DictionaryTypeCreateInput input)
    {
        var entity = input.Adapt<DictionaryTypeEntity>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<DictionaryTypeDto>();
    }

    public async Task<DictionaryTypeDto> UpdateAsync(Guid id, DictionaryTypeUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<DictionaryTypeDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }
}