using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hugin.IdentityServer.EntityExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Hugin.IdentityServer.Security
{
    public class HuginUserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory
    {
        public HuginUserClaimsPrincipalFactory(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options,
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory)
            : base(userManager, roleManager, options, currentPrincipalAccessor, abpClaimsPrincipalFactory)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = principal.Identities.First();

            if (!user.GetUserOpenId().IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim("wechat", user.GetUserOpenId()));
            }

            if (!user.GetUserStadiumId().IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim("stadiums", user.GetUserStadiumId()));
            }

            return principal;
        }
    }
}
