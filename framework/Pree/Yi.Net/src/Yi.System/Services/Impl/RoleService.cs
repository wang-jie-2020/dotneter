using Yi.Framework.Abstractions;
using Yi.Framework.Core.Entities;
using Yi.System.Domains;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class RoleService : BaseService, IRoleService
{
    private readonly RoleManager _roleManager;

    private readonly ISqlSugarRepository<RoleEntity> _repository;
    private readonly ISqlSugarRepository<RoleDeptEntity> _roleDeptRepository;
    private readonly ISqlSugarRepository<UserRoleEntity> _userRoleRepository;

    public RoleService(RoleManager roleManager,
        ISqlSugarRepository<RoleDeptEntity> roleDeptRepository,
        ISqlSugarRepository<UserRoleEntity> userRoleRepository,
        ISqlSugarRepository<RoleEntity> repository)
    {
        (_roleManager, _roleDeptRepository, _userRoleRepository, _repository) =
            (roleManager, roleDeptRepository, userRoleRepository, repository);
    }

    public async Task<RoleDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<RoleDto>();
    }

    public async Task<PagedResult<RoleDto>> GetListAsync(RoleQuery query)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable().WhereIF(!string.IsNullOrEmpty(query.RoleCode),
                x => x.RoleCode.Contains(query.RoleCode!))
            .WhereIF(!string.IsNullOrEmpty(query.RoleName), x => x.RoleName.Contains(query.RoleName!))
            .WhereIF(query.State is not null, x => x.State == query.State)
            .ToPageListAsync(query.PageNum, query.PageSize, total);
        return new PagedResult<RoleDto>(total, entities.Adapt<List<RoleDto>>());
    }

    /// <summary>
    ///     添加角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<RoleDto> CreateAsync(RoleInput input)
    {
        var entity = input.Adapt<RoleEntity>();
        await _repository.InsertAsync(entity);
        await _roleManager.GiveRoleSetMenuAsync(new List<Guid> { entity.Id }, input.MenuIds);

        return entity.Adapt<RoleDto>();
        ;
    }

    /// <summary>
    ///     修改角色
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<RoleDto> UpdateAsync(Guid id, RoleInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);

        await _repository.UpdateAsync(entity);
        await _roleManager.GiveRoleSetMenuAsync(new List<Guid> { id }, input.MenuIds);

        return entity.Adapt<RoleDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
    
    public async Task UpdateDataScopeAsync(UpdateDataScopeInput input)
    {
        //只有自定义的需要特殊处理
        if (input.DataScope == DataScopeEnum.CUSTOM)
        {
            await _roleDeptRepository.DeleteAsync(x => x.RoleId == input.RoleId);

            var insertEntities = input.DeptIds.Select(x => new RoleDeptEntity { DeptId = x, RoleId = input.RoleId }).ToList();
            await _roleDeptRepository.InsertRangeAsync(insertEntities);
        }

        var entity = new RoleEntity { DataScope = input.DataScope };
        entity.Id = input.RoleId;

        await _repository.AsUpdateable(entity).UpdateColumns(x => x.DataScope).ExecuteCommandAsync();
    }

    /// <summary>
    ///     更新状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public async Task<RoleDto> UpdateStateAsync(Guid id, bool state)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) throw new ArgumentOutOfRangeException();

        entity.State = state;
        await _repository.UpdateAsync(entity);
        return entity.Adapt<RoleDto>();
    }

    /// <summary>
    ///     获取角色下的用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="query"></param>
    /// <param name="isAllocated">是否在该角色下</param>
    /// <returns></returns>
    public async Task<PagedResult<UserDto>> GetAuthUserByRoleIdAsync(Guid roleId, bool isAllocated, RoleAuthUserQuery query)
    {
        PagedResult<UserDto> output;
        //角色下已授权用户
        if (isAllocated)
        {
            output = await GetAllocatedAuthUserByRoleIdAsync(roleId, query);
        }
        //角色下未授权用户
        else
        {
            output = await GetNotAllocatedAuthUserByRoleIdAsync(roleId, query);
        }

        return output;
    }

    private async Task<PagedResult<UserDto>> GetAllocatedAuthUserByRoleIdAsync(Guid roleId, RoleAuthUserQuery query)
    {
        RefAsync<int> total = 0;

        var output = await _userRoleRepository.AsQueryable()
            .LeftJoin<UserEntity>((ur, u) => ur.UserId == u.Id && ur.RoleId == roleId)
            .Where((ur, u) => ur.RoleId == roleId)
            .WhereIF(!string.IsNullOrEmpty(query.UserName), (ur, u) => u.UserName.Contains(query.UserName))
            .WhereIF(query.Phone is not null, (ur, u) => u.Phone.ToString().Contains(query.Phone.ToString()))
            .Select((ur, u) => new UserDto { Id = u.Id }, true)
            .ToPageListAsync(query.PageSize, query.PageNum, total);

        return new PagedResult<UserDto>(total, output);
    }

    private async Task<PagedResult<UserDto>> GetNotAllocatedAuthUserByRoleIdAsync(Guid roleId, RoleAuthUserQuery query)
    {
        RefAsync<int> total = 0;

        var entities = await _userRoleRepository.Context.Queryable<UserEntity>()
            .Where(u => SqlFunc.Subqueryable<UserRoleEntity>().Where(x => x.RoleId == roleId)
                .Where(x => x.UserId == u.Id).NotAny())
            .WhereIF(!string.IsNullOrEmpty(query.UserName), u => u.UserName.Contains(query.UserName))
            .WhereIF(query.Phone is not null, u => u.Phone.ToString().Contains(query.Phone.ToString()))
            .ToPageListAsync(query.PageSize, query.PageNum, total);

        var output = entities.Adapt<List<UserDto>>();
        return new PagedResult<UserDto>(total, output);
    }

    /// <summary>
    ///     批量给用户授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task CreateAuthUserAsync(RoleAuthUserInput input)
    {
        var userRoleEntities = input.UserIds.Select(u => new UserRoleEntity { RoleId = input.RoleId, UserId = u })
            .ToList();
        await _userRoleRepository.InsertRangeAsync(userRoleEntities);
    }

    /// <summary>
    ///     批量取消授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task DeleteAuthUserAsync(RoleAuthUserInput input)
    {
        await _userRoleRepository.Context.Deleteable<UserRoleEntity>().Where(x => x.RoleId == input.RoleId)
            .Where(x => input.UserIds.Contains(x.UserId))
            .ExecuteCommandAsync();
    }
}