using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Admin.Controllers.System;

[ApiController]
[Route("api/system/language")]
public class LanguageController(ILanguageService languageService) : BaseController
{
    [HttpGet("{id}")]
    public async Task<LanguageDto> GetAsync(long id)
    {
        return await languageService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<LanguageDto>> GetListAsync([FromQuery] LanguageQuery query)
    {
        return await languageService.GetListAsync(query);
    }

    [HttpPost]
    public async Task<LanguageDto> CreateAsync([FromBody] LanguageInput input)
    {
        return await languageService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<LanguageDto> UpdateAsync(long id, [FromBody] LanguageInput input)
    {
        return await languageService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<long> id)
    {
        await languageService.DeleteAsync(id);
    }
}
