using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.OperationLogging.Dtos;
using Yi.Infra.OperationLogging.Entities;

namespace Yi.Infra.OperationLogging.Services;

/// <summary>
///     OperationLog服务实现
/// </summary>
public class OperationLogService : YiCrudAppService<OperationLogEntity, OperationLogGetListOutput, Guid,
        OperationLogGetListInput>,
    IOperationLogService
{
    private readonly ISqlSugarRepository<OperationLogEntity, Guid> _repository;

    public OperationLogService(ISqlSugarRepository<OperationLogEntity, Guid> repository) : base(repository)
    {
        _repository = repository;
    }

    public override async Task<PagedResultDto<OperationLogGetListOutput>> GetListAsync(
        OperationLogGetListInput input)
    {
        RefAsync<int> total = 0;
        var entities = await _repository._DbQueryable.WhereIF(!string.IsNullOrEmpty(input.OperUser),
                x => x.OperUser.Contains(input.OperUser!))
            .WhereIF(input.OperType is not null, x => x.OperType == input.OperType)
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
        return new PagedResultDto<OperationLogGetListOutput>(total, await MapToGetListOutputDtosAsync(entities));
    }

    [RemoteService(false)]
    public override Task<OperationLogGetListOutput> UpdateAsync(Guid id, OperationLogGetListOutput input)
    {
        return base.UpdateAsync(id, input);
    }
}