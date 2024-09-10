using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.OperationLogging.Dtos;
using Yi.Infra.OperationLogging.Entities;

namespace Yi.Infra.OperationLogging.Services;

/// <summary>
///     OperationLog服务实现
/// </summary>
public class OperationLogService : YiCrudAppService<OperationLogEntity, OperationLogDto, Guid,
        OperationLogGetListInput>,
    IOperationLogService
{
    private readonly ISqlSugarRepository<OperationLogEntity, Guid> _repository;

    public OperationLogService(ISqlSugarRepository<OperationLogEntity, Guid> repository) : base(repository)
    {
        _repository = repository;
    }

    public override async Task<PagedResultDto<OperationLogDto>> GetListAsync(
        OperationLogGetListInput input)
    {
        RefAsync<int> total = 0;
        var entities = await _repository._DbQueryable.WhereIF(!string.IsNullOrEmpty(input.OperUser),
                x => x.OperUser.Contains(input.OperUser!))
            .WhereIF(input.OperType is not null, x => x.OperType == input.OperType)
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
        return new PagedResultDto<OperationLogDto>(total, await MapToGetListOutputDtosAsync(entities));
    }

    [RemoteService(false)]
    public override Task<OperationLogDto> UpdateAsync(Guid id, OperationLogDto input)
    {
        return base.UpdateAsync(id, input);
    }
}