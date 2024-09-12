using Yi.Infra.Rbac.Dtos;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     Dept服务抽象
/// </summary>
public interface IDeptService : IYiCrudAppService<DeptGetOutputDto, DeptGetListOutputDto, Guid, DeptGetListInputVo,
    DeptCreateInputVo, DeptUpdateInputVo>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
}