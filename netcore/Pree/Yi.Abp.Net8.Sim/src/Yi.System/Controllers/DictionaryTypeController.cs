using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.System.Services.Sys;
using Yi.System.Services.Sys.Dtos;

namespace Yi.System.Controllers;

[ApiController]
[Route("api/dictionary-type")]
public class DictionaryTypeController : AbpController
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
    public async Task<PagedResultDto<DictionaryTypeDto>> GetListAsync([FromQuery] DictionaryTypeGetListInput input)
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