using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Settings.Dtos;
using Yi.Infra.Settings.Entities;

namespace Yi.Infra.Settings.Services;

/// <summary>
///     DictionaryType服务实现
/// </summary>
public class DictionaryTypeService : YiCrudAppService<DictionaryTypeAggregateRoot, DictionaryTypeDto,
        DictionaryTypeDto, Guid, DictionaryTypeGetListInput, DictionaryTypeCreateInput,
        DictionaryTypeUpdateInput>,
    IDictionaryTypeService
{
    private readonly ISqlSugarRepository<DictionaryTypeAggregateRoot, Guid> _repository;

    public DictionaryTypeService(ISqlSugarRepository<DictionaryTypeAggregateRoot, Guid> repository) : base(repository)
    {
        _repository = repository;
    }

    public override async Task<PagedResultDto<DictionaryTypeDto>> GetListAsync(
        DictionaryTypeGetListInput input)
    {
        RefAsync<int> total = 0;
        var entities = await _repository._DbQueryable
            .WhereIF(input.DictName is not null, x => x.DictName.Contains(input.DictName!))
            .WhereIF(input.DictType is not null, x => x.DictType!.Contains(input.DictType!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<DictionaryTypeDto>
        {
            TotalCount = total,
            Items = await MapToGetListOutputDtosAsync(entities)
        };
    }
}