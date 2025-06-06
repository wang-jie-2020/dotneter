using Volo.Abp.Application.Services;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra;

public interface IAccountService : IApplicationService
{
    Task<UserRoleMenuDto> GetAsync();
    
    Task<CaptchaImageDto> GetCaptchaImageAsync();
    
    Task<object> PostLoginAsync(LoginInputVo input);
    
    Task PostRegisterAsync(RegisterDto input);
    
    Task<bool> RestPasswordAsync(Guid userId, RestPasswordDto input);
}