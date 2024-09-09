using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Framework.Ddd.Application;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Dtos.OperLog;
using Yi.Infra.Rbac.IServices;
using Yi.Infra.Rbac.Operlog;

namespace Yi.Infra.Rbac.Services.RecordLog;

/// <summary>
///     OperationLog服务实现
/// </summary>
public class OperationLogService : YiCrudAppService<OperationLogEntity, OperationLogGetListOutputDto, Guid,
        OperationLogGetListInputVo>,
    IOperationLogService
{
    private readonly ISqlSugarRepository<OperationLogEntity, Guid> _repository;

    public OperationLogService(ISqlSugarRepository<OperationLogEntity, Guid> repository) : base(repository)
    {
        _repository = repository;
    }

    public override async Task<PagedResultDto<OperationLogGetListOutputDto>> GetListAsync(
        OperationLogGetListInputVo input)
    {
        RefAsync<int> total = 0;
        var entities = await _repository._DbQueryable.WhereIF(!string.IsNullOrEmpty(input.OperUser),
                x => x.OperUser.Contains(input.OperUser!))
            .WhereIF(input.OperType is not null, x => x.OperType == input.OperType)
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
        return new PagedResultDto<OperationLogGetListOutputDto>(total, await MapToGetListOutputDtosAsync(entities));
    }

    [RemoteService(false)]
    public override Task<OperationLogGetListOutputDto> UpdateAsync(Guid id, OperationLogGetListOutputDto input)
    {
        return base.UpdateAsync(id, input);
    }
}