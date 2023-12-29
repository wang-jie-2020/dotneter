using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Volo.Abp.IdentityServer;

namespace Hugin.IdentityServer.Security
{
    public class HuginClaimsService : AbpClaimsService
    {
        public HuginClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }

        protected override IEnumerable<string> FilterRequestedClaimTypes(IEnumerable<string> claimTypes)
        {
            return base.FilterRequestedClaimTypes(claimTypes).Union(new[] { "wechat", "stadiums" });
        }
    }
}



