using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Core;
using Yi.Framework.Core.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Web.Controllers.System;

[ApiController]
[Route(("api/system/menu"))]
public class MenuController : BaseController
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet("{id}")]
    public async Task<MenuDto> GetAsync(Guid id)
    {
        return await _menuService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<MenuDto>> GetListAsync([FromQuery] MenuGetListInput input)
    {
        return await _menuService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<MenuDto> CreateAsync([FromBody] MenuCreateInput input)
    {
        return await _menuService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<MenuDto> UpdateAsync(Guid id, [FromBody] MenuUpdateInput input)
    {
        return await _menuService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _menuService.DeleteAsync(id);
    }
    
    /// <summary>
    ///     查询当前角色的菜单
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpGet("role-id/{roleId}")]
    public async Task<List<MenuDto>> GetListRoleIdAsync([FromRoute] Guid roleId)
    {
        return await _menuService.GetListRoleIdAsync(roleId);
    }
}