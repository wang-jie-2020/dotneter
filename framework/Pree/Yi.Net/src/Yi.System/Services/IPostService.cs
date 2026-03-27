using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IPostService
{
    Task<PostDto> GetAsync(long id);

    Task<PagedResult<PostDto>> GetListAsync(PostQuery query);

    Task<PostDto> CreateAsync(PostInput input);

    Task<PostDto> UpdateAsync(long id, PostInput input);

    Task DeleteAsync(IEnumerable<long> id);
}