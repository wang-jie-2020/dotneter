using Microsoft.AspNetCore.Mvc;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IUserService
{
    Task<UserDetailDto> GetAsync(Guid id);

    Task<PagedResult<UserDto>> GetListAsync(UserQuery query);

    Task<UserDto> CreateAsync(UserInput input);

    Task<UserDto> UpdateAsync(Guid id, UserInput input);

    Task DeleteAsync(IEnumerable<Guid> id);

    Task<IActionResult> GetExportExcelAsync(UserQuery query);

    Task<IActionResult> GetImportTemplateAsync();
    
    Task PostImportExcelAsync(Stream input);

    /// <summary>
    ///     更新个人中心
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<UserDto> UpdateProfileAsync(ProfileInput input);

    /// <summary>
    ///     更新状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    Task<UserDto> UpdateStateAsync(Guid id, bool state);
}