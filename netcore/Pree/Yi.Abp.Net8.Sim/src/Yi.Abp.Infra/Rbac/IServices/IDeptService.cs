using Yi.Abp.Infra.Rbac.Dtos.Dept;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Abp.Infra.Rbac.IServices
{
    /// <summary>
    /// Dept服务抽象
    /// </summary>
    public interface IDeptService : IYiCrudAppService<DeptGetOutputDto, DeptGetListOutputDto, Guid, DeptGetListInputVo, DeptCreateInputVo, DeptUpdateInputVo>
    {
        Task<List<Guid>> GetChildListAsync(Guid deptId);
    }
}
