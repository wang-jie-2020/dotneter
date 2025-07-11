using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Admin.Controllers.System;

[ApiController]
[Route("api/system/dictionary")]
public class DictionaryController : BaseController
{
    private readonly IDictionaryService _dictionaryService;


    public DictionaryController(IDictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }

    [HttpGet("{id}")]
    public async Task<DictionaryDto> GetAsync(Guid id)
    {
        return await _dictionaryService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<DictionaryDto>> GetListAsync([FromQuery] DictionaryQuery query)
    {
        return await _dictionaryService.GetListAsync(query);
    }

    [HttpPost]
    public async Task<DictionaryDto> CreateAsync([FromBody] DictionaryInput input)
    {
        return await _dictionaryService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<DictionaryDto> UpdateAsync(Guid id, [FromBody] DictionaryInput input)
    {
        return await _dictionaryService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _dictionaryService.DeleteAsync(id);
    }
    
    /// <summary>
    ///     根据字典类型获取字典列表
    /// </summary>
    /// <param name="dicType"></param>
    /// <returns></returns>
    [HttpGet("dic-type/{dicType}")]
    public async Task<List<DictionaryDto>> GetDicType([FromRoute] string dicType)
    {
        return await _dictionaryService.GetDicType(dicType);
    }
}