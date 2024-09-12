using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.Infra.Settings.Dtos;
using Yi.Infra.Settings.Services;
using Yi.Infra.TenantManagement.Dtos;
using Yi.Infra.TenantManagement.Services;

namespace Yi.Infra.Settings.Controllers;

[ApiController]
[Route("api/app/dictionary")]
public class DictionaryController : AbpController
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
    public async Task<PagedResultDto<DictionaryDto>> GetListAsync([FromQuery] DictionaryGetListInput input)
    {
        return await _dictionaryService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<DictionaryDto> CreateAsync([FromBody] DictionaryCreateInput input)
    {
        return await _dictionaryService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<DictionaryDto> UpdateAsync(Guid id, [FromBody] DictionaryUpdateInput input)
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
    [Route("dic-type/{dicType}")]
    public async Task<List<DictionaryDto>> GetDicType([FromRoute] string dicType)
    {
        return await _dictionaryService.GetDicType(dicType);
    }
}