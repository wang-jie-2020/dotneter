using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.System.Services.Rbac.Dtos;
using Yi.System.Services.Rbac.Entities;
using Yi.System.Services.Rbac.Managers;

namespace Yi.System.Services.Rbac.Services;

[RemoteService(false)]
public class UserService : ApplicationService, IUserService
{
    private readonly ISqlSugarRepository<UserAggregateRoot, Guid> _repository;
    private readonly UserManager _userManager;
    private readonly IDeptService _deptService;

    public UserService(ISqlSugarRepository<UserAggregateRoot, Guid> repository,
        UserManager userManager,
        IDeptService deptService)
    {
        _repository = repository;
        _userManager = userManager;
        _deptService = deptService;
    }

    public async Task<UserGetOutputDto> GetAsync(Guid id)
    {
        //使用导航树形查询
        var entity = await _repository.DbQueryable
            .Includes(u => u.Roles)
            .Includes(u => u.Posts)
            .Includes(u => u.Dept)
            .InSingleAsync(id);

        return entity.Adapt<UserGetOutputDto>();
    }

    public async Task<PagedResultDto<UserGetListOutputDto>> GetListAsync(UserGetListInput input)
    {
        RefAsync<int> total = 0;

        List<Guid> deptIds = null;
        if (input.DeptId is not null)
        {
            deptIds = await _deptService.GetChildListAsync(input.DeptId ?? Guid.Empty);
        }

        ;

        var ids = input.Ids?.Split(",").Select(x => Guid.Parse(x)).ToList();

        var outPut = await _repository.DbQueryable
            .WhereIF(!string.IsNullOrEmpty(input.UserName), x => x.UserName.Contains(input.UserName!))
            .WhereIF(input.Phone is not null, x => x.Phone.ToString()!.Contains(input.Phone.ToString()!))
            .WhereIF(!string.IsNullOrEmpty(input.Name), x => x.Name!.Contains(input.Name!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .WhereIF(input.DeptId is not null, x => deptIds.Contains(x.DeptId ?? Guid.Empty))
            .WhereIF(ids is not null, x => ids.Contains(x.Id))
            .LeftJoin<DeptAggregateRoot>((user, dept) => user.DeptId == dept.Id)
            .OrderByDescending(user => user.CreationTime)
            .Select((user, dept) => new UserGetListOutputDto(), true)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        var result = new PagedResultDto<UserGetListOutputDto>
        {
            Items = outPut,
            TotalCount = total
        };
        return result;
    }

    public async Task<UserGetOutputDto> CreateAsync(UserCreateInput input)
    {
        var entity = input.Adapt<UserAggregateRoot>();
        entity.BuildPassword(input.Password);

        await _userManager.CreateAsync(entity);
        await _userManager.GiveUserSetRoleAsync(new List<Guid> { entity.Id }, input.RoleIds);
        await _userManager.GiveUserSetPostAsync(new List<Guid> { entity.Id }, input.PostIds);

        return entity.Adapt<UserGetOutputDto>();
    }

    public async Task<UserGetOutputDto> UpdateAsync(Guid id, UserUpdateInput input)
    {
        if (input.UserName == UserConst.Admin || input.UserName == UserConst.TenantAdmin)
        {
            throw new UserFriendlyException(UserConst.Name_Not_Allowed);
        }

        if (await _repository.IsAnyAsync(u => input.UserName!.Equals(u.UserName) && !id.Equals(u.Id)))
        {
            throw new UserFriendlyException("用户已经存在，更新失败");
        }

        var entity = await _repository.GetByIdAsync(id);
        //更新密码，特殊处理
        if (input.Password is not null)
        {
            entity.EncryPassword.Password = input.Password;
            entity.BuildPassword();
        }

        input.Adapt(entity);
        await _repository.UpdateAsync(entity);
        await _userManager.GiveUserSetRoleAsync(new List<Guid> { id }, input.RoleIds);
        await _userManager.GiveUserSetPostAsync(new List<Guid> { id }, input.PostIds);

        return entity.Adapt<UserGetOutputDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(UserGetListInput input)
    {
        if (input is IPagedResultRequest paged)
        {
            paged.SkipCount = 0;
            paged.MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount;
        }

        var output = await GetListAsync(input);
        return new PhysicalFileResult(ExporterHelper.ExportExcel(output.Items), "application/vnd.ms-excel");
    }

    public Task PostImportExcelAsync(List<UserCreateInput> input)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     更新个人中心
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<UserGetOutputDto> UpdateProfileAsync(ProfileUpdateInput input)
    {
        var entity = await _repository.GetByIdAsync(CurrentUser.Id);
        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity);
        return entity.Adapt<UserGetOutputDto>();
    }

    /// <summary>
    ///     更新状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public async Task<UserGetOutputDto> UpdateStateAsync(Guid id, bool state)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
        {
            throw new ApplicationException("用户未存在");
        }

        entity.State = state;
        await _repository.UpdateAsync(entity);
        return entity.Adapt<UserGetOutputDto>();
    }
}