using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IPostService
{
    Task<PostDto> GetAsync(Guid id);

    Task<PagedResult<PostDto>> GetListAsync(PostQuery query);

    Task<PostDto> CreateAsync(PostInput input);

    Task<PostDto> UpdateAsync(Guid id, PostInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}