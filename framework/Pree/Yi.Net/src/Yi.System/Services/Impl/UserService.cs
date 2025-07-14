using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using Yi.AspNetCore;
using Yi.Framework.Abstractions;
using Yi.Framework.Core.Entities;
using Yi.System.Domains;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class UserService : BaseService, IUserService
{
    private readonly ISqlSugarRepository<UserEntity> _repository;
    private readonly UserManager _userManager;
    private readonly IDeptService _deptService;

    public UserService(ISqlSugarRepository<UserEntity> repository,
        UserManager userManager,
        IDeptService deptService)
    {
        _repository = repository;
        _userManager = userManager;
        _deptService = deptService;
    }

    public async Task<UserDetailDto> GetAsync(Guid id)
    {
        //使用导航树形查询
        var entity = await _repository.AsQueryable()
            .Includes(u => u.Roles)
            .Includes(u => u.Posts)
            .Includes(u => u.Dept)
            .InSingleAsync(id);

        return entity.Adapt<UserDetailDto>();
    }

    public async Task<PagedResult<UserDto>> GetListAsync(UserQuery query)
    {
        RefAsync<int> total = 0;

        List<Guid> deptIds = null;
        if (query.DeptId is not null)
        {
            deptIds = await _deptService.GetChildListAsync(query.DeptId ?? Guid.Empty);
        }

        ;

        var ids = query.Ids?.Split(",").Select(x => Guid.Parse(x)).ToList();

        var outPut = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(query.UserName), x => x.UserName.Contains(query.UserName!))
            .WhereIF(query.Phone is not null, x => x.Phone.ToString()!.Contains(query.Phone.ToString()!))
            .WhereIF(!string.IsNullOrEmpty(query.Name), x => x.Name!.Contains(query.Name!))
            .WhereIF(query.State is not null, x => x.State == query.State)
            .WhereIF(query.StartTime is not null && query.EndTime is not null, x => x.CreationTime >= query.StartTime && x.CreationTime <= query.EndTime)
            .WhereIF(query.DeptId is not null, x => deptIds.Contains(x.DeptId ?? Guid.Empty))
            .WhereIF(ids is not null, x => ids.Contains(x.Id))
            .LeftJoin<DeptEntity>((user, dept) => user.DeptId == dept.Id)
            .OrderByDescending(user => user.CreationTime)
            .Select((user, dept) => new UserDto(), true)
            .ToPageListAsync(query.PageNum, query.PageSize, total);

        var result = new PagedResult<UserDto>
        {
            Items = outPut,
            TotalCount = total
        };
        return result;
    }

    public async Task<UserDto> CreateAsync(UserInput input)
    {
        var entity = input.Adapt<UserEntity>();
        //entity.BuildPassword(input.Password);

        await _userManager.CreateAsync(entity);
        await _userManager.GiveUserSetRoleAsync(new List<Guid> { entity.Id }, input.RoleIds);
        await _userManager.GiveUserSetPostAsync(new List<Guid> { entity.Id }, input.PostIds);

        return entity.Adapt<UserDto>();
    }

    public async Task<UserDto> UpdateAsync(Guid id, UserInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _userManager.UpdateAsync(entity);
        await _userManager.GiveUserSetRoleAsync(new List<Guid> { id }, input.RoleIds);
        await _userManager.GiveUserSetPostAsync(new List<Guid> { id }, input.PostIds);

        return entity.Adapt<UserDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }

    public async Task<IActionResult> GetExportExcelAsync(UserQuery query)
    {
        if (query is PagedQuery paged)
        {
            paged.PageNum = 0;
            paged.PageSize = int.MaxValue;
        }

        var output = await GetListAsync(query);

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
        await MiniExcel.SaveAsAsync(stream, new List<UserInput>());
        stream.Seek(0, SeekOrigin.Begin);

        return new FileStreamResult(stream, "application/vnd.ms-excel")
        {
            FileDownloadName = $"{L[nameof(UserEntity)]}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}" + ".xlsx"
        };
    }

    public async Task PostImportExcelAsync(Stream stream)
    {
        var rows = await MiniExcel.QueryAsync<UserInput>(stream);
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
    public async Task<UserDto> UpdateProfileAsync(ProfileInput input)
    {
        var entity = await _repository.GetByIdAsync(CurrentUser.Id);
        input.Adapt(entity);

        await _repository.UpdateAsync(entity);
        return entity.Adapt<UserDto>();
    }

    /// <summary>
    ///     更新状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public async Task<UserDto> UpdateStateAsync(Guid id, bool state)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
        {
            throw new EntityNotFoundException(typeof(UserEntity), id);
        }

        entity.State = state;
        await _repository.UpdateAsync(entity);
        return entity.Adapt<UserDto>();
    }
}