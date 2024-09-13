using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.Infra.Rbac.Dtos;
using Yi.Infra.Rbac.Services;

namespace Yi.Infra.Rbac.Controllers;

[ApiController]
[Route("api/app/post")]
public class PostController : AbpController
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("{id}")]
    public async Task<PostDto> GetAsync(Guid id)
    {
        return await _postService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<PostDto>> GetListAsync([FromQuery] PostGetListInput input)
    {
        return await _postService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<PostDto> CreateAsync([FromBody] PostCreateInput input)
    {
        return await _postService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<PostDto> UpdateAsync(Guid id, [FromBody] PostUpdateInput input)
    {
        return await _postService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _postService.DeleteAsync(id);
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] PostGetListInput input)
    {
        return await _postService.GetExportExcelAsync(input);
    }
}