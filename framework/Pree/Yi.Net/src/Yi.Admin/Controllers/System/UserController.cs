using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Core;
using Yi.Framework.Core.Abstractions;
using Yi.Framework.Loggings;
using Yi.Framework.Permissions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Web.Controllers.System;

[ApiController]
[Route("api/system/user")]
public class UserController : BaseController
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
    public async Task<PagedResult<UserGetListOutputDto>> GetListAsync([FromQuery] UserGetListInput input)
    {
        return await _userService.GetListAsync(input);
    }

    [HttpPost]
    [OperLog("添加用户", OperLogEnum.Insert)]
    [Permission("system:user:add")]
    public async Task<UserGetOutputDto> CreateAsync([FromBody] UserCreateInput input)
    {
        return await _userService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    [OperLog("更新用户", OperLogEnum.Update)]
    [Permission("system:user:edit")]
    public async Task<UserGetOutputDto> UpdateAsync(Guid id, [FromBody] UserUpdateInput input)
    {
        return await _userService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [OperLog("删除用户", OperLogEnum.Delete)]
    [Permission("system:user:delete")]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _userService.DeleteAsync(id);
    }

    [HttpGet("export")]
    [Permission("system:user:export")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] UserGetListInput input)
    {
        return await _userService.GetExportExcelAsync(input);
    }

    [HttpGet("template")]
    public async Task<IActionResult> GetImportTemplateAsync()
    {
        return await _userService.GetImportTemplateAsync();
    }
    
    [HttpPost("import")]
    [Permission("system:user:import")]
    public Task PostImportExcelAsync(IFormFile file)
    {
        return _userService.PostImportExcelAsync(file.OpenReadStream());
    }

    /// <summary>
    ///     更新个人中心
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("profile")]
    [OperLog("更新个人信息", OperLogEnum.Update)]
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
    [OperLog("更新用户状态", OperLogEnum.Update)]
    [Permission("system:user:update")]
    public async Task<UserGetOutputDto> UpdateStateAsync([FromRoute] Guid id, [FromRoute] bool state)
    {
        return await _userService.UpdateStateAsync(id, state);
    }
}