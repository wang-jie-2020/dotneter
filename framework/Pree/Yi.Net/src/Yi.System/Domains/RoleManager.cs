using Yi.Framework.Abstractions;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Domains.Entities;

namespace Yi.System.Domains;

public class RoleManager : BaseDomain
{
    private ISqlSugarRepository<RoleEntity> _repository;
    private readonly ISqlSugarRepository<RoleMenuEntity> _roleMenuRepository;

    public RoleManager(ISqlSugarRepository<RoleEntity> repository,
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
        //这个是需要事务的，在service中进行工作单元
        await _roleMenuRepository.DeleteAsync(u => roleIds.Contains(u.RoleId));
        //遍历用户
        foreach (var roleId in roleIds)
        {
            //添加新的关系
            List<RoleMenuEntity> roleMenuEntity = new();
            foreach (var menu in menuIds)
            {
                roleMenuEntity.Add(new RoleMenuEntity { RoleId = roleId, MenuId = menu });
            }
            //一次性批量添加
            await _roleMenuRepository.InsertRangeAsync(roleMenuEntity);
        }
    }
}