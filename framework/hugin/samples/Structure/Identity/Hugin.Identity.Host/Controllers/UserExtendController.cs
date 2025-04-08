using System;
using System.Threading.Tasks;
using Hugin.Identity.Users;
using Hugin.IdentityServer.EntityExtensions;
using Hugin.IdentityServer.RepositoryExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Hugin.IdentityServer.Controllers
{
    [Area("identity")]
    [ControllerName("UserExtend")]
    [Route("api/identity/user-extend")]
    [Authorize]
    public class UserExtendController : AbpController, IUserExtendAppService
    {
        protected IdentityUserManager UserManager { get; }

        protected IIdentityUserRepository UserRepository { get; }

        public UserExtendController(IdentityUserManager userManager,
            IIdentityUserAppService userAppService,
            IIdentityUserRepository userRepository)
        {
            UserManager = userManager;
            UserRepository = userRepository;
        }

        [HttpPost]
        [Route("{id}/openId")]
        public async Task<IdentityUserDto> SetUserOpenId(Guid id, [FromForm] string openId)
        {
            var user = await UserRepository.GetAsync(id);
            user.SetUserOpenId(openId);
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [HttpGet]
        [Route("get-openId")]
        public async Task<IdentityUserDto> GetUserByOpenId(string openId)
        {
            var user = await UserRepository.GetUserByOpenId(openId);
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [HttpPost]
        [Route("{id}/stadiumId")]
        public async Task<IdentityUserDto> SetUserStadiumId(Guid id, [FromForm] string[] stadiumIds)
        {
            var user = await UserRepository.GetAsync(id);
            user.SetUserStadiumId(stadiumIds);
            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }
    }
}