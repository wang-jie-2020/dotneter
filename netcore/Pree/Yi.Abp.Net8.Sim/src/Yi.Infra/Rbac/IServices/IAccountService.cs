using Volo.Abp.Application.Services;
using Yi.Infra.Rbac.Dtos;
using Yi.Infra.Rbac.Dtos.Account;

namespace Yi.Infra.Rbac.IServices
{
    public interface IAccountService : IApplicationService
    {
        Task<UserRoleMenuDto> GetAsync();
        Task<CaptchaImageDto> GetCaptchaImageAsync();
        Task<object> PostLoginAsync(LoginInputVo input);
        Task PostRegisterAsync(RegisterDto input);
        Task<bool> RestPasswordAsync(Guid userId, RestPasswordDto input);
    }
}
