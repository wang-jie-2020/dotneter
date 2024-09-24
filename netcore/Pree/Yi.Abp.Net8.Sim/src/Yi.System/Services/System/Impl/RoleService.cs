using Microsoft.AspNetCore.Mvc;
using Utils.Exporting;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Yi.AspNetCore.Helpers;
using Yi.AspNetCore.System;
using Yi.System.Domains.System;
using Yi.System.Domains.System.Entities;
using Yi.System.Services.System.Dtos;

namespace Yi.System.Services.System.Impl;

public class RoleService : ApplicationService, IRoleService
{
    private readonly RoleManager _roleManager;

    private readonly ISqlSugarRepository<RoleEntity, Guid> _repository;
    private readonly ISqlSugarRepository<RoleDeptEntity> _roleDeptRepository;
    private readonly ISqlSugarRepository<UserRoleEntity> _userRoleRepository;

    public RoleService(RoleManager roleManager,
        ISqlSugarRepository<RoleDeptEntity> roleDeptRepository,
        ISqlSugarRepository<UserRoleEntity> userRoleRepository,
        ISqlSugarRepository<RoleEntity, Guid> repository)
    {
        (_roleManager, _roleDeptRepository, _userRoleRepository, _repository) =
            (roleManager, roleDeptRepository, userRoleRepository, repository);
    }

    public async Task<RoleDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<RoleDto>();
    }

    public async Task<PagedResultDto<RoleDto>> GetListAsync(RoleGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable.WhereIF(!string.IsNullOrEmpty(input.RoleCode),
                x => x.RoleCode.Contains(input.RoleCode!))
            .WhereIF(!string.IsNullOrEmpty(input.RoleName), x => x.RoleName.Contains(input.RoleName!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .ToPageListAsync(input.PageNum, input.PageSize, total);
        return new PagedResultDto<RoleDto>(total, entities.Adapt<List<RoleDto>>());
    }

    /// <summary>
    ///     添加角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<RoleDto> CreateAsync(RoleCreateInput input)
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
    public async Task<RoleDto> UpdateAsync(Guid id, RoleUpdateInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);

        await _repository.UpdateAsync(entity);
        await _roleManager.GiveRoleSetMenuAsync(new List<Guid> { id }, input.MenuIds);

        return entity.Adapt<RoleDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(RoleGetListInput input)
    {
        if (input is PagedInput paged)
        {
            paged.PageNum = 0;
            paged.PageSize = int.MaxValue;
        }

        var output = await GetListAsync(input);
        return new PhysicalFileResult(ExporterHelper.ExportExcel(output.Items), "application/vnd.ms-excel");
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
        EntityHelper.TrySetId(entity, () => input.RoleId);

        await _repository.Db.Updateable(entity).UpdateColumns(x => x.DataScope).ExecuteCommandAsync();
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
        if (entity is null) throw new ApplicationException("角色未存在");

        entity.State = state;
        await _repository.UpdateAsync(entity);
        return entity.Adapt<RoleDto>();
    }

    /// <summary>
    ///     获取角色下的用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="input"></param>
    /// <param name="isAllocated">是否在该角色下</param>
    /// <returns></returns>
    public async Task<PagedResultDto<UserGetListOutputDto>> GetAuthUserByRoleIdAsync(Guid roleId, bool isAllocated, RoleAuthUserGetListInput input)
    {
        PagedResultDto<UserGetListOutputDto> output;
        //角色下已授权用户
        if (isAllocated)
        {
            output = await GetAllocatedAuthUserByRoleIdAsync(roleId, input);
        }
        //角色下未授权用户
        else
        {
            output = await GetNotAllocatedAuthUserByRoleIdAsync(roleId, input);
        }

        return output;
    }

    private async Task<PagedResultDto<UserGetListOutputDto>> GetAllocatedAuthUserByRoleIdAsync(Guid roleId, RoleAuthUserGetListInput input)
    {
        RefAsync<int> total = 0;

        var output = await _userRoleRepository.DbQueryable
            .LeftJoin<UserEntity>((ur, u) => ur.UserId == u.Id && ur.RoleId == roleId)
            .Where((ur, u) => ur.RoleId == roleId)
            .WhereIF(!string.IsNullOrEmpty(input.UserName), (ur, u) => u.UserName.Contains(input.UserName))
            .WhereIF(input.Phone is not null, (ur, u) => u.Phone.ToString().Contains(input.Phone.ToString()))
            .Select((ur, u) => new UserGetListOutputDto { Id = u.Id }, true)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<UserGetListOutputDto>(total, output);
    }

    private async Task<PagedResultDto<UserGetListOutputDto>> GetNotAllocatedAuthUserByRoleIdAsync(Guid roleId, RoleAuthUserGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _userRoleRepository.Db.Queryable<UserEntity>()
            .Where(u => SqlFunc.Subqueryable<UserRoleEntity>().Where(x => x.RoleId == roleId)
                .Where(x => x.UserId == u.Id).NotAny())
            .WhereIF(!string.IsNullOrEmpty(input.UserName), u => u.UserName.Contains(input.UserName))
            .WhereIF(input.Phone is not null, u => u.Phone.ToString().Contains(input.Phone.ToString()))
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        var output = entities.Adapt<List<UserGetListOutputDto>>();
        return new PagedResultDto<UserGetListOutputDto>(total, output);
    }

    /// <summary>
    ///     批量给用户授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task CreateAuthUserAsync(RoleAuthUserCreateOrDeleteInput input)
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
    public async Task DeleteAuthUserAsync(RoleAuthUserCreateOrDeleteInput input)
    {
        await _userRoleRepository.Db.Deleteable<UserRoleEntity>().Where(x => x.RoleId == input.RoleId)
            .Where(x => input.UserIds.Contains(x.UserId))
            .ExecuteCommandAsync();
    }
}