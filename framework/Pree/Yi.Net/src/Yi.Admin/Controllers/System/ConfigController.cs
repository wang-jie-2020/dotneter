using Microsoft.AspNetCore.Mvc;
using Yi.AspNetCore.Core;
using Yi.Framework.Core;
using Yi.Framework.Core.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Web.Controllers.System;

[ApiController]
[Route("api/system/config")]
public class ConfigController : BaseController
{
    private readonly IConfigService _configService;

    public ConfigController(IConfigService configService)
    {
        _configService = configService;
    }

    [HttpGet("{id}")]
    public async Task<ConfigGetOutputDto> GetAsync(Guid id)
    {
        return await _configService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<ConfigGetListOutputDto>> GetListAsync([FromQuery] ConfigGetListInputVo input)
    {
        return await _configService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<ConfigGetOutputDto> CreateAsync([FromBody] ConfigCreateInputVo input)
    {
        return await _configService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<ConfigGetOutputDto> UpdateAsync(Guid id, [FromBody] ConfigUpdateInput input)
    {
        return await _configService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _configService.DeleteAsync(id);
    }
}