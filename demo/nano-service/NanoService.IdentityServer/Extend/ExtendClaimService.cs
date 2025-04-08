using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServer4.Validation;

namespace NanoService.IdentityServer.Extend
{
    /*
     *  IClaimService 是为identity token 和 access token 建立claims集合的，同时调用IProfileService来赋值
     */
    public class ExtendClaimService : DefaultClaimsService, IClaimsService
    {
        public ExtendClaimService(IProfileService profile, ILogger<DefaultClaimsService> logger)
            : base(profile, logger)
        {
        }

        protected override IEnumerable<string> FilterRequestedClaimTypes(IEnumerable<string> claimTypes)
        {
            var claims = new[] { "name" };
            return base.FilterRequestedClaimTypes(claimTypes).Union(claims);
        }
    }
}
