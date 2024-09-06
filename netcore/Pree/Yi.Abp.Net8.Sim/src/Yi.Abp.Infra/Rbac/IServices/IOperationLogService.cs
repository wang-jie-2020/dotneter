using Yi.Abp.Infra.Rbac.Dtos.OperLog;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// OperationLog服务抽象
    /// </summary>
    public interface IOperationLogService : IYiCrudAppService<OperationLogGetListOutputDto, Guid, OperationLogGetListInputVo>
    {

    }
}
