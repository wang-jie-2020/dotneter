using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.Sys.Domains.Infra.Entities;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra.Impl;

public class ConfigService : ApplicationService, IConfigService
{
    private readonly ISqlSugarRepository<ConfigEntity, Guid> _repository;

    public ConfigService(ISqlSugarRepository<ConfigEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<ConfigGetOutputDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<ConfigGetOutputDto>();
    }

    public async Task<PagedResultDto<ConfigGetListOutputDto>> GetListAsync(ConfigGetListInputVo input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable.WhereIF(!string.IsNullOrEmpty(input.ConfigKey), x => x.ConfigKey.Contains(input.ConfigKey!))
            .WhereIF(!string.IsNullOrEmpty(input.ConfigName), x => x.ConfigName!.Contains(input.ConfigName!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.PageNum, input.PageSize, total);

        return new PagedResultDto<ConfigGetListOutputDto>(total, entities.Adapt<List<ConfigGetListOutputDto>>());
    }

    public async Task<ConfigGetOutputDto> CreateAsync(ConfigCreateInputVo input)
    {
        var entity = input.Adapt<ConfigEntity>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<ConfigGetOutputDto>();
    }

    public async Task<ConfigGetOutputDto> UpdateAsync(Guid id, ConfigUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<ConfigGetOutputDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }
}