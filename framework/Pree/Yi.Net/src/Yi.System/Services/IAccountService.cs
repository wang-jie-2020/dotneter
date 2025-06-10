using Volo.Abp.Application.Services;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IAccountService
{
    Task<UserRoleMenuDto> GetAsync();
    
    Task<CaptchaImageDto> GetCaptchaImageAsync();
    
    Task<object> PostLoginAsync(LoginInputVo input);
    
    Task PostRegisterAsync(RegisterDto input);
    
    Task<bool> RestPasswordAsync(Guid userId, RestPasswordDto input);
}