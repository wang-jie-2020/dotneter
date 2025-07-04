using Yi.Framework.Abstractions;
using Yi.System.Domains.Entities;

namespace Yi.System.Domains;

public class RoleManager : BaseDomain
{
    private readonly ISqlSugarRepository<RoleEntity> _repository;
    private readonly ISqlSugarRepository<RoleMenuEntity> _roleMenuRepository;

    public RoleManager(
        ISqlSugarRepository<RoleEntity> repository,
        ISqlSugarRepository<RoleMenuEntity> roleMenuRepository)
    {
        _repository = repository;
        _roleMenuRepository = roleMenuRepository;
    }

    /// <summary>
    ///     给角色设置菜单
    /// </summary>
    /// <param name="roleIds"></param>
    /// <param name="menuIds"></param>
    /// <returns></returns>
    public async Task GiveRoleSetMenuAsync(List<Guid> roleIds, List<Guid> menuIds)
    {
        await _roleMenuRepository.DeleteAsync(u => roleIds.Contains(u.RoleId));
        foreach (var roleId in roleIds)
        {
            List<RoleMenuEntity> roleMenuEntity = new();
            foreach (var menu in menuIds)
            {
                roleMenuEntity.Add(new RoleMenuEntity { RoleId = roleId, MenuId = menu });
            }
            await _roleMenuRepository.InsertRangeAsync(roleMenuEntity);
        }
    }
}