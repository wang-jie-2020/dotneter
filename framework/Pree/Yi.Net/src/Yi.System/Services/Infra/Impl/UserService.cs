using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Yi.AspNetCore.Core;
using Yi.Sys.Domains.Infra;
using Yi.Sys.Domains.Infra.Consts;
using Yi.Sys.Domains.Infra.Entities;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra.Impl;

public class UserService : ApplicationService, IUserService
{
    private readonly ISqlSugarRepository<UserEntity, Guid> _repository;
    private readonly UserManager _userManager;
    private readonly IDeptService _deptService;

    public UserService(ISqlSugarRepository<UserEntity, Guid> repository,
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
            .LeftJoin<DeptEntity>((user, dept) => user.DeptId == dept.Id)
            .OrderByDescending(user => user.CreationTime)
            .Select((user, dept) => new UserGetListOutputDto(), true)
            .ToPageListAsync(input.PageNum, input.PageSize, total);

        var result = new PagedResultDto<UserGetListOutputDto>
        {
            Items = outPut,
            TotalCount = total
        };
        return result;
    }

    public async Task<UserGetOutputDto> CreateAsync(UserCreateInput input)
    {
        var entity = input.Adapt<UserEntity>();
        entity.BuildPassword(input.Password);

        await _userManager.CreateAsync(entity);
        await _userManager.GiveUserSetRoleAsync(new List<Guid> { entity.Id }, input.RoleIds);
        await _userManager.GiveUserSetPostAsync(new List<Guid> { entity.Id }, input.PostIds);

        return entity.Adapt<UserGetOutputDto>();
    }

    public async Task<UserGetOutputDto> UpdateAsync(Guid id, UserUpdateInput input)
    {
        if (input.UserName == AccountConst.Admin || input.UserName == AccountConst.TenantAdmin)
        {
            throw Oops.Oh(AccountConst.User_Name_Not_Allowed);
        }

        if (await _repository.IsAnyAsync(u => input.UserName!.Equals(u.UserName) && !id.Equals(u.Id)))
        {
            throw Oops.Oh("Name_Repeat");
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
        if (input is PagedInput paged)
        {
            paged.PageNum = 0;
            paged.PageSize = int.MaxValue;
        }

        var output = await GetListAsync(input);

        var stream = new MemoryStream();
        await MiniExcel.SaveAsAsync(stream, output.Items);
        stream.Seek(0, SeekOrigin.Begin);

        return new FileStreamResult(stream, "application/vnd.ms-excel")
        {
            FileDownloadName = $"{L[nameof(UserEntity)]}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}" + ".xlsx"
        };
    }

    public async Task<IActionResult> GetImportTemplateAsync()
    {
        var stream = new MemoryStream();
        await MiniExcel.SaveAsAsync(stream, new List<UserCreateInput>());
        stream.Seek(0, SeekOrigin.Begin);

        return new FileStreamResult(stream, "application/vnd.ms-excel")
        {
            FileDownloadName = $"{L[nameof(UserEntity)]}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}" + ".xlsx"
        };
    }
    
    public async Task PostImportExcelAsync(Stream stream)
    {
        var rows = await MiniExcel.QueryAsync<UserCreateInput>(stream);
        foreach (var row in rows)
        {
            Console.WriteLine(row.ToString());
        }
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
            throw Oops.Oh(AccountConst.User_Not_Exist);
        }

        entity.State = state;
        await _repository.UpdateAsync(entity);
        return entity.Adapt<UserGetOutputDto>();
    }
}