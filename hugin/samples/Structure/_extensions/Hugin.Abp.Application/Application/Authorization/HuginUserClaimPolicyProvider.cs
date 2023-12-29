using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Hugin.Application.Authorization
{
    public class HuginUserClaimPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public HuginUserClaimPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy != null)
            {
                return policy;
            }

            //var scopes = new[] { "platform", "terminal" };

            //var scope = scopes.FirstOrDefault(p => p == policyName);
            //if (scope != null)
            //{
            //    var policyBuilder = new AuthorizationPolicyBuilder(Array.Empty<string>());
            //    policyBuilder.Requirements.Add(new HuginUserClaimRequirement(policyName));
            //    return policyBuilder.Build();
            //}

            return null;
        }
    }
}
