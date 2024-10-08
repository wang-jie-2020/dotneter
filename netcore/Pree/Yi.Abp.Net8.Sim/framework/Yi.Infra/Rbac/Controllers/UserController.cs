using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.Infra.OperationLogging;
using Yi.Infra.Permissions;
using Yi.Infra.Rbac.Dtos;
using Yi.Infra.Rbac.Services;

namespace Yi.Infra.Rbac.Controllers;

[ApiController]
[Route("api/app/user")]
public class UserController : AbpController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<UserGetOutputDto> GetAsync(Guid id)
    {
        return await _userService.GetAsync(id);
    }

    [HttpGet]
    [Permission("system:user:list")]
    public async Task<PagedResultDto<UserGetListOutputDto>> GetListAsync([FromQuery] UserGetListInput input)
    {
        return await _userService.GetListAsync(input);
    }

    [HttpPost]
    [OperationLog("添加用户", OperationEnum.Insert)]
    [Permission("system:user:add")]
    public async Task<UserGetOutputDto> CreateAsync([FromBody] UserCreateInput input)
    {
        return await _userService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    [OperationLog("更新用户", OperationEnum.Update)]
    [Permission("system:user:edit")]
    public async Task<UserGetOutputDto> UpdateAsync(Guid id, [FromBody] UserUpdateInput input)
    {
        return await _userService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [OperationLog("删除用户", OperationEnum.Delete)]
    [Permission("system:user:delete")]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _userService.DeleteAsync(id);
    }

    [HttpGet("export-excel")]
    [Permission("system:user:export")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] UserGetListInput input)
    {
        return await _userService.GetExportExcelAsync(input);
    }

    [HttpPost("import-excel")]
    [Permission("system:user:import")]
    public Task PostImportExcelAsync([FromBody] List<UserCreateInput> input)
    {
        return _userService.PostImportExcelAsync(input);
    }

    /// <summary>
    ///     更新个人中心
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("profile")]
    [OperationLog("更新个人信息", OperationEnum.Update)]
    public async Task<UserGetOutputDto> UpdateProfileAsync(ProfileUpdateInput input)
    {
        return await _userService.UpdateProfileAsync(input);
    }

    /// <summary>
    ///     更新状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [HttpPut("{id}/{state}")]
    [OperationLog("更新用户状态", OperationEnum.Update)]
    [Permission("system:user:update")]
    public async Task<UserGetOutputDto> UpdateStateAsync([FromRoute] Guid id, [FromRoute] bool state)
    {
        return await _userService.UpdateStateAsync(id, state);
    }
}