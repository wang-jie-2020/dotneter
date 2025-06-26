using Yi.Framework;
using Yi.Framework.Abstractions;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Domains.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class ConfigService : BaseService, IConfigService
{
    private readonly ISqlSugarRepository<ConfigEntity> _repository;

    public ConfigService(ISqlSugarRepository<ConfigEntity> repository)
    {
        _repository = repository;
    }

    public async Task<ConfigGetOutputDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<ConfigGetOutputDto>();
    }

    public async Task<PagedResult<ConfigGetListOutputDto>> GetListAsync(ConfigGetListInputVo input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable().WhereIF(!string.IsNullOrEmpty(input.ConfigKey), x => x.ConfigKey.Contains(input.ConfigKey!))
            .WhereIF(!string.IsNullOrEmpty(input.ConfigName), x => x.ConfigName!.Contains(input.ConfigName!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.PageNum, input.PageSize, total);

        return new PagedResult<ConfigGetListOutputDto>(total, entities.Adapt<List<ConfigGetListOutputDto>>());
    }

    public async Task<ConfigGetOutputDto> CreateAsync(ConfigCreateInputVo input)
    {
        var entity = input.Adapt<ConfigEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<ConfigGetOutputDto>();
    }

    public async Task<ConfigGetOutputDto> UpdateAsync(Guid id, ConfigUpdateInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<ConfigGetOutputDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
}