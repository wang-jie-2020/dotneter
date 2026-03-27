using Yi.Framework.Abstractions;
using Yi.Framework.Core.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class LanguageService(ISqlSugarRepository<LanguageEntity> repository) : BaseService, ILanguageService
{
    public async Task<LanguageDto> GetAsync(long id)
    {
        var entity = await repository.GetByIdAsync(id);
        return entity.Adapt<LanguageDto>();
    }

    public async Task<PagedResult<LanguageDto>> GetListAsync(LanguageQuery query)
    {
        RefAsync<int> total = 0;
        var entities = await repository.AsQueryable()
            .WhereIF(query.Name is not null, x => x.Name.Contains(query.Name!))
            .WhereIF(query.Value is not null, x => x.Value.Contains(query.Value!))
            .WhereIF(query.Culture is not null, x => x.Culture.Contains(query.Culture!))
            .WhereIF(query.StartTime is not null && query.EndTime is not null, x => x.CreationTime >= query.StartTime && x.CreationTime <= query.EndTime)
            .ToPageListAsync(query.PageNum, query.PageSize, total);

        return new PagedResult<LanguageDto>
        {
            TotalCount = total,
            Items = entities.Adapt<List<LanguageDto>>()
        };
    }

    public async Task<LanguageDto> CreateAsync(LanguageInput input)
    {
        var entity = input.Adapt<LanguageEntity>();
        await repository.InsertAsync(entity);

        return entity.Adapt<LanguageDto>();
    }

    public async Task<LanguageDto> UpdateAsync(long id, LanguageInput input)
    {
        var entity = await repository.GetByIdAsync(id);
        input.Adapt(entity);
        await repository.UpdateAsync(entity);

        return entity.Adapt<LanguageDto>();
    }

    public async Task DeleteAsync(IEnumerable<long> id)
    {
        await repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
}
