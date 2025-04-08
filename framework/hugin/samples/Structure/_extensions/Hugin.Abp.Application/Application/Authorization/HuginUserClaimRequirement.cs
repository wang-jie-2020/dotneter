using Microsoft.AspNetCore.Authorization;

namespace Hugin.Application.Authorization
{
    public class HuginUserClaimRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }

        public string ClaimValue { get; }

        public HuginUserClaimRequirement(string claimValue, string claimType = "")
        {
            ClaimValue = claimValue;
            ClaimType = claimType;
        }
    }
}