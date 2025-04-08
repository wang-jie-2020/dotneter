using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Hugin.Application.Authorization
{
    public class HuginUserClaimHandler : AuthorizationHandler<HuginUserClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HuginUserClaimRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Value == requirement.ClaimValue &&
                                    (string.IsNullOrWhiteSpace(requirement.ClaimType) || x.Type == requirement.ClaimType)))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}