using Volo.Abp.Application.Services;
using Yi.System.Account.Dtos;
using Yi.System.Rbac.Dtos;

namespace Yi.System.Account;

public interface IAccountService : IApplicationService
{
    Task<UserRoleMenuDto> GetAsync();
    
    Task<CaptchaImageDto> GetCaptchaImageAsync();
    
    Task<object> PostLoginAsync(LoginInputVo input);
    
    Task PostRegisterAsync(RegisterDto input);
    
    Task<bool> RestPasswordAsync(Guid userId, RestPasswordDto input);
}