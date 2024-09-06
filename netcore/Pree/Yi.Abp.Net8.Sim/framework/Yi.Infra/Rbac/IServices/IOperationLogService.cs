using Yi.Framework.Ddd.Application.Contracts;
using Yi.Infra.Rbac.Dtos.OperLog;

namespace Yi.Infra.Rbac.IServices
{
    /// <summary>
    /// OperationLog服务抽象
    /// </summary>
    public interface IOperationLogService : IYiCrudAppService<OperationLogGetListOutputDto, Guid, OperationLogGetListInputVo>
    {

    }
}
