using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.System.Services.System.Dtos;

namespace Yi.System.Services.System;

public interface IPostService
{
    Task<PostDto> GetAsync(long id);

    Task<PagedResultDto<PostDto>> GetListAsync(PostGetListInput input);

    Task<PostDto> CreateAsync(PostCreateInput input);

    Task<PostDto> UpdateAsync(long id, PostUpdateInput input);

    Task DeleteAsync(IEnumerable<long> id);
}