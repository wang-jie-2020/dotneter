using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra;

public interface IUserService
{
    Task<UserGetOutputDto> GetAsync(Guid id);

    Task<PagedResultDto<UserGetListOutputDto>> GetListAsync(UserGetListInput input);

    Task<UserGetOutputDto> CreateAsync(UserCreateInput input);

    Task<UserGetOutputDto> UpdateAsync(Guid id, UserUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(UserGetListInput input);

    Task<IActionResult> GetImportTemplateAsync();
    
    Task PostImportExcelAsync(Stream input);

    /// <summary>
    ///     更新个人中心
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<UserGetOutputDto> UpdateProfileAsync(ProfileUpdateInput input);

    /// <summary>
    ///     更新状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    Task<UserGetOutputDto> UpdateStateAsync(Guid id, bool state);
}