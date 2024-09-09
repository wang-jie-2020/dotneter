using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Users;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Authorization;
using Yi.Infra.Rbac.Caches;
using Yi.Infra.Rbac.Consts;
using Yi.Infra.Rbac.Dtos.User;
using Yi.Infra.Rbac.Entities;
using Yi.Infra.Rbac.Entities.ValueObjects;
using Yi.Infra.Rbac.IServices;
using Yi.Infra.Rbac.Managers;
using Yi.Infra.Rbac.Operlog;
using Yi.Infra.Rbac.Repositories;

namespace Yi.Infra.Rbac.Services.System;

/// <summary>
///     User服务实现
/// </summary>
public class UserService : YiCrudAppService<UserAggregateRoot, UserGetOutputDto, UserGetListOutputDto, Guid,
        UserGetListInputVo, UserCreateInputVo, UserUpdateInputVo>, IUserService
    //IUserService
{
    private ILocalEventBus _localEventBus;
    private readonly ISqlSugarRepository<UserAggregateRoot, Guid> _repository;

    public UserService(ISqlSugarRepository<UserAggregateRoot, Guid> repository, UserManager userManager,
        IUserRepository userRepository, ICurrentUser currentUser, IDeptService deptService,
        ILocalEventBus localEventBus, IDistributedCache<UserInfoCacheItem, UserInfoCacheKey> userCache) : base(
        repository)
    {
        (_userManager, _userRepository, _currentUser, _deptService, _repository, _localEventBus) =
            (userManager, userRepository, currentUser, deptService, repository, localEventBus);
    }

    private UserManager _userManager { get; }
    private IUserRepository _userRepository { get; set; }
    private IDeptService _deptService { get; }

    private ICurrentUser _currentUser { get; }

    /// <summary>
    ///     查询用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Permission("system:user:list")]
    public override async Task<PagedResultDto<UserGetListOutputDto>> GetListAsync(UserGetListInputVo input)
    {
        RefAsync<int> total = 0;
        List<Guid> deptIds = null;
        if (input.DeptId is not null) deptIds = await _deptService.GetChildListAsync(input.DeptId ?? Guid.Empty);


        var ids = input.Ids?.Split(",").Select(x => Guid.Parse(x)).ToList();
        var outPut = await _repository._DbQueryable
            .WhereIF(!string.IsNullOrEmpty(input.UserName), x => x.UserName.Contains(input.UserName!))
            .WhereIF(input.Phone is not null, x => x.Phone.ToString()!.Contains(input.Phone.ToString()!))
            .WhereIF(!string.IsNullOrEmpty(input.Name), x => x.Name!.Contains(input.Name!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)

            //这个为过滤当前部门，加入数据权限后，将由数据权限控制
            .WhereIF(input.DeptId is not null, x => deptIds.Contains(x.DeptId ?? Guid.Empty))
            .WhereIF(ids is not null, x => ids.Contains(x.Id))
            .LeftJoin<DeptAggregateRoot>((user, dept) => user.DeptId == dept.Id)
            .OrderByDescending(user => user.CreationTime)
            .Select((user, dept) => new UserGetListOutputDto(), true)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        var result = new PagedResultDto<UserGetListOutputDto>();
        result.Items = outPut;
        result.TotalCount = total;
        return result;
    }

    /// <summary>
    ///     添加用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperLog("添加用户", OperEnum.Insert)]
    [Permission("system:user:add")]
    public override async Task<UserGetOutputDto> CreateAsync(UserCreateInputVo input)
    {
        var entitiy = await MapToEntityAsync(input);

        await _userManager.CreateAsync(entitiy);
        await _userManager.GiveUserSetRoleAsync(new List<Guid> { entitiy.Id }, input.RoleIds);
        await _userManager.GiveUserSetPostAsync(new List<Guid> { entitiy.Id }, input.PostIds);

        var result = await MapToGetOutputDtoAsync(entitiy);
        return result;
    }

    /// <summary>
    ///     单查
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override async Task<UserGetOutputDto> GetAsync(Guid id)
    {
        //使用导航树形查询
        var entity = await _repository._DbQueryable.Includes(u => u.Roles).Includes(u => u.Posts).Includes(u => u.Dept)
            .InSingleAsync(id);

        return await MapToGetOutputDtoAsync(entity);
    }

    /// <summary>
    ///     更新用户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperLog("更新用户", OperEnum.Update)]
    [Permission("system:user:edit")]
    public override async Task<UserGetOutputDto> UpdateAsync(Guid id, UserUpdateInputVo input)
    {
        if (input.UserName == UserConst.Admin || input.UserName == UserConst.TenantAdmin)
            throw new UserFriendlyException(UserConst.Name_Not_Allowed);
        if (await _repository.IsAnyAsync(u => input.UserName!.Equals(u.UserName) && !id.Equals(u.Id)))
            throw new UserFriendlyException("用户已经存在，更新失败");
        var entity = await _repository.GetByIdAsync(id);
        //更新密码，特殊处理
        if (input.Password is not null)
        {
            entity.EncryPassword.Password = input.Password;
            entity.BuildPassword();
        }

        await MapToEntityAsync(input, entity);

        var res1 = await _repository.UpdateAsync(entity);
        await _userManager.GiveUserSetRoleAsync(new List<Guid> { id }, input.RoleIds);
        await _userManager.GiveUserSetPostAsync(new List<Guid> { id }, input.PostIds);
        return await MapToGetOutputDtoAsync(entity);
    }

    [OperLog("删除用户", OperEnum.Delete)]
    [Permission("system:user:delete")]
    public override async Task DeleteAsync(Guid id)
    {
        await base.DeleteAsync(id);
    }


    protected override UserAggregateRoot MapToEntity(UserCreateInputVo createInput)
    {
        var output = base.MapToEntity(createInput);
        output.EncryPassword = new EncryPasswordValueObject(createInput.Password);
        return output;
    }

    protected override async Task<UserAggregateRoot> MapToEntityAsync(UserCreateInputVo createInput)
    {
        var entitiy = await base.MapToEntityAsync(createInput);
        entitiy.BuildPassword();
        return entitiy;
    }

    /// <summary>
    ///     更新个人中心
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [OperLog("更新个人信息", OperEnum.Update)]
    public async Task<UserGetOutputDto> UpdateProfileAsync(ProfileUpdateInputVo input)
    {
        var entity = await _repository.GetByIdAsync(_currentUser.Id);
        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity);
        var dto = await MapToGetOutputDtoAsync(entity);
        return dto;
    }

    /// <summary>
    ///     更新状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [Route("user/{id}/{state}")]
    [OperLog("更新用户状态", OperEnum.Update)]
    [Permission("system:user:update")]
    public async Task<UserGetOutputDto> UpdateStateAsync([FromRoute] Guid id, [FromRoute] bool state)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) throw new ApplicationException("用户未存在");
        entity.State = state;
        await _repository.UpdateAsync(entity);
        return await MapToGetOutputDtoAsync(entity);
    }

    [Permission("system:user:export")]
    public override Task<IActionResult> GetExportExcelAsync(UserGetListInputVo input)
    {
        return base.GetExportExcelAsync(input);
    }

    [Permission("system:user:import")]
    public override Task PostImportExcelAsync(List<UserCreateInputVo> input)
    {
        return base.PostImportExcelAsync(input);
    }
}