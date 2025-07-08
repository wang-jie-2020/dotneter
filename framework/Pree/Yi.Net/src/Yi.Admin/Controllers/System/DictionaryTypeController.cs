using Microsoft.AspNetCore.Mvc;
using Yi.Framework;
using Yi.Framework.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Web.Controllers.System;

[ApiController]
[Route("api/system/dictionary-type")]
public class DictionaryTypeController : BaseController
{
    private readonly IDictionaryTypeService _dictionaryTypeService;


    public DictionaryTypeController(IDictionaryTypeService dictionaryTypeService)
    {
        _dictionaryTypeService = dictionaryTypeService;
    }

    [HttpGet("{id}")]
    public async Task<DictionaryTypeDto> GetAsync(Guid id)
    {
        return await _dictionaryTypeService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<DictionaryTypeDto>> GetListAsync([FromQuery] DictionaryTypeQuery query)
    {
        return await _dictionaryTypeService.GetListAsync(query);
    }

    [HttpPost]
    public async Task<DictionaryTypeDto> CreateAsync([FromBody] DictionaryTypeInput input)
    {
        return await _dictionaryTypeService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<DictionaryTypeDto> UpdateAsync(Guid id, [FromBody] DictionaryTypeInput input)
    {
        return await _dictionaryTypeService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _dictionaryTypeService.DeleteAsync(id);
    }
}