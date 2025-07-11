using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;
using ConfigQuery = Yi.System.Services.Dtos.ConfigQuery;

namespace Yi.Admin.Controllers.System;

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
    public async Task<ConfigDto> GetAsync(Guid id)
    {
        return await _configService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<ConfigDto>> GetListAsync([FromQuery] ConfigQuery query)
    {
        return await _configService.GetListAsync(query);
    }

    [HttpPost]
    public async Task<ConfigDto> CreateAsync([FromBody] ConfigInput input)
    {
        return await _configService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<ConfigDto> UpdateAsync(Guid id, [FromBody] ConfigInput input)
    {
        return await _configService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _configService.DeleteAsync(id);
    }
}