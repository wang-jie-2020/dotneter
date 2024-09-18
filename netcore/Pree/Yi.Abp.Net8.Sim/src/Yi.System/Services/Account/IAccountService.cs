using Volo.Abp.Application.Services;
using Yi.System.Services.Account.Dtos;
using Yi.System.Services.Rbac.Dtos;

namespace Yi.System.Services.Account;

public interface IAccountService : IApplicationService
{
    Task<UserRoleMenuDto> GetAsync();
    
    Task<CaptchaImageDto> GetCaptchaImageAsync();
    
    Task<object> PostLoginAsync(LoginInputVo input);
    
    Task PostRegisterAsync(RegisterDto input);
    
    Task<bool> RestPasswordAsync(Guid userId, RestPasswordDto input);
}