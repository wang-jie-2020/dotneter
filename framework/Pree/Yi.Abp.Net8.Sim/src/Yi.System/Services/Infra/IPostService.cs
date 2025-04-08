using Volo.Abp.Application.Dtos;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra;

public interface IPostService
{
    Task<PostDto> GetAsync(Guid id);

    Task<PagedResultDto<PostDto>> GetListAsync(PostGetListInput input);

    Task<PostDto> CreateAsync(PostCreateInput input);

    Task<PostDto> UpdateAsync(Guid id, PostUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
}