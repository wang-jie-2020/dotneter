using Microsoft.AspNetCore.Mvc;
using Yi.AspNetCore.Core;
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
    public async Task<PagedResult<DictionaryTypeDto>> GetListAsync([FromQuery] DictionaryTypeGetListInput input)
    {
        return await _dictionaryTypeService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<DictionaryTypeDto> CreateAsync([FromBody] DictionaryTypeCreateInput input)
    {
        return await _dictionaryTypeService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<DictionaryTypeDto> UpdateAsync(Guid id, [FromBody] DictionaryTypeUpdateInput input)
    {
        return await _dictionaryTypeService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _dictionaryTypeService.DeleteAsync(id);
    }
}