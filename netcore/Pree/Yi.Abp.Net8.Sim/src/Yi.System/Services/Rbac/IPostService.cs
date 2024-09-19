using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.System.Services.Rbac.Dtos;

namespace Yi.System.Services.Rbac;

public interface IPostService
{
    Task<PostDto> GetAsync(Guid id);

    Task<PagedResultDto<PostDto>> GetListAsync(PostGetListInput input);

    Task<PostDto> CreateAsync(PostCreateInput input);

    Task<PostDto> UpdateAsync(Guid id, PostUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(PostGetListInput input);
}