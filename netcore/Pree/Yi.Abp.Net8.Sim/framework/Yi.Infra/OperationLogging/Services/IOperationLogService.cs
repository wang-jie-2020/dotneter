using Yi.Infra.OperationLogging.Dtos;

namespace Yi.Infra.OperationLogging.Services;

/// <summary>
///     OperationLog服务抽象
/// </summary>
public interface IOperationLogService : IYiCrudAppService<OperationLogDto, Guid, OperationLogGetListInput>
{
}