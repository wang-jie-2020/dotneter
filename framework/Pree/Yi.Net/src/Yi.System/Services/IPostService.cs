using Yi.AspNetCore.Core;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IPostService
{
    Task<PostDto> GetAsync(Guid id);

    Task<PagedResult<PostDto>> GetListAsync(PostGetListInput input);

    Task<PostDto> CreateAsync(PostCreateInput input);

    Task<PostDto> UpdateAsync(Guid id, PostUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}