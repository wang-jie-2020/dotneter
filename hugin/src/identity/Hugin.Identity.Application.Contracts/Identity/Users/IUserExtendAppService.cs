using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace Hugin.Identity.Users
{
    public interface IUserExtendAppService : IApplicationService
    {
        Task<IdentityUserDto> SetUserOpenId(Guid id, string openId);

        Task<IdentityUserDto> GetUserByOpenId(string openId);

        Task<IdentityUserDto> SetUserStadiumId(Guid id, string[] stadiumIds);
    }
}
