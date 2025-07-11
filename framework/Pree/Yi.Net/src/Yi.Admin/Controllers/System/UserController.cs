using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yi.AspNetCore.Mvc.OperLogging;
using Yi.Framework;
using Yi.Framework.Abstractions;
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
    public async Task<UserDetailDto> GetAsync(Guid id)
    {
        return await _userService.GetAsync(id);
    }

    [HttpGet]
    [Authorize("system:user:list")]
    public async Task<PagedResult<UserDto>> GetListAsync([FromQuery] UserQuery query)
    {
        return await _userService.GetListAsync(query);
    }

    [HttpPost]
    [OperLog("添加用户", OperLogEnum.Insert)]
    [Authorize("system:user:add")]
    public async Task<UserDto> CreateAsync([FromBody] UserInput input)
    {
        return await _userService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    [OperLog("更新用户", OperLogEnum.Update)]
    [Authorize("system:user:edit")]
    public async Task<UserDto> UpdateAsync(Guid id, [FromBody] UserInput input)
    {
        return await _userService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [OperLog("删除用户", OperLogEnum.Delete)]
    [Authorize("system:user:delete")]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _userService.DeleteAsync(id);
    }

    [HttpGet("export")]
    [Authorize("system:user:export")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] UserQuery query)
    {
        return await _userService.GetExportExcelAsync(query);
    }

    [HttpGet("template")]
    public async Task<IActionResult> GetImportTemplateAsync()
    {
        return await _userService.GetImportTemplateAsync();
    }
    
    [HttpPost("import")]
    [Authorize("system:user:import")]
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
    public async Task<UserDto> UpdateProfileAsync(ProfileInput input)
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
    [Authorize("system:user:update")]
    public async Task<UserDto> UpdateStateAsync([FromRoute] Guid id, [FromRoute] bool state)
    {
        return await _userService.UpdateStateAsync(id, state);
    }
}