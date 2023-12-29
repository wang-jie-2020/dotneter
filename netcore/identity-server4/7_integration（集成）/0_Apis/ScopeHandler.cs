using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Apis
{
    public class ScopeHandler : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Value == requirement.ScopeName))
                context.Succeed(requirement);


            return Task.CompletedTask;
        }
    }
}
