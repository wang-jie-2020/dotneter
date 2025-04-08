using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.Sys.Services.Infra;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Admin.Controllers.Sys;

[ApiController]
[Route("api/system/post")]
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
}