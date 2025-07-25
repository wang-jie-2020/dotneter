using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Admin.Controllers.System;

[ApiController]
[Route("api/system/post")]
public class PostController : BaseController
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
    public async Task<PagedResult<PostDto>> GetListAsync([FromQuery] PostQuery query)
    {
        return await _postService.GetListAsync(query);
    }

    [HttpPost]
    public async Task<PostDto> CreateAsync([FromBody] PostInput input)
    {
        return await _postService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<PostDto> UpdateAsync(Guid id, [FromBody] PostInput input)
    {
        return await _postService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _postService.DeleteAsync(id);
    }
}