using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.Infra.Rbac.Dtos;

namespace Yi.Infra.Rbac.Services;

public interface IPostService
{
    Task<PostDto> GetAsync(Guid id);

    Task<PagedResultDto<PostDto>> GetListAsync(PostGetListInput input);

    Task<PostDto> CreateAsync(PostCreateInput input);

    Task<PostDto> UpdateAsync(Guid id, PostUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(PostGetListInput input);
}