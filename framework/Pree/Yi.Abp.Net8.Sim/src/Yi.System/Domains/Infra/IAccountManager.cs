using Volo.Abp.Domain.Services;
using Yi.Sys.Domains.Infra.Entities;

namespace Yi.Sys.Domains.Infra;

public interface IAccountManager : IDomainService
{
    string CreateRefreshToken(Guid userId);
    
    Task<string> GetTokenByUserIdAsync(Guid userId);
    
    Task LoginValidationAsync(string userName, string password, Action<UserEntity> userAction = null);
    
    Task RegisterAsync(string userName, string password, long phone);
    
    Task<bool> RestPasswordAsync(Guid userId, string password);
    
    Task UpdatePasswordAsync(Guid userId, string newPassword, string oldPassword);
}