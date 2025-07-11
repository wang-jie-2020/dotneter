using Yi.Framework.Abstractions;
using Yi.Framework.Core.Entities;
using Yi.System.Entities;
using Yi.System.Services.Dtos;
using ConfigQuery = Yi.System.Services.Dtos.ConfigQuery;

namespace Yi.System.Services.Impl;

public class ConfigService : BaseService, IConfigService
{
    private readonly ISqlSugarRepository<ConfigEntity> _repository;

    public ConfigService(ISqlSugarRepository<ConfigEntity> repository)
    {
        _repository = repository;
    }

    public async Task<ConfigDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<ConfigDto>();
    }

    public async Task<PagedResult<ConfigDto>> GetListAsync(ConfigQuery query)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable().WhereIF(!string.IsNullOrEmpty(query.ConfigKey), x => x.ConfigKey.Contains(query.ConfigKey!))
            .WhereIF(!string.IsNullOrEmpty(query.ConfigName), x => x.ConfigName!.Contains(query.ConfigName!))
            .WhereIF(query.StartTime is not null && query.EndTime is not null, x => x.CreationTime >= query.StartTime && x.CreationTime <= query.EndTime)
            .ToPageListAsync(query.PageNum, query.PageSize, total);

        return new PagedResult<ConfigDto>(total, entities.Adapt<List<ConfigDto>>());
    }

    public async Task<ConfigDto> CreateAsync(ConfigInput input)
    {
        var entity = input.Adapt<ConfigEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<ConfigDto>();
    }

    public async Task<ConfigDto> UpdateAsync(Guid id, ConfigInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<ConfigDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
}