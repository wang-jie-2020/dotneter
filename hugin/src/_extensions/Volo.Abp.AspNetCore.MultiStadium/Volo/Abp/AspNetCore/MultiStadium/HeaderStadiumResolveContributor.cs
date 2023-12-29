using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    public class HeaderStadiumResolveContributor : HttpStadiumResolveContributorBase
    {
        public const string ContributorName = "Header";

        public override string Name => ContributorName;

        protected override Task<string> GetStadiumIdOrNameFromHttpContextOrNullAsync(IStadiumResolveContext context, HttpContext httpContext)
        {
            if (httpContext.Request.Headers.IsNullOrEmpty())
            {
                return Task.FromResult((string)null);
            }

            var tenantIdKey = context.GetAbpAspNetCoreMultiStadiumOptions().StadiumKey;

            var tenantIdHeader = httpContext.Request.Headers[tenantIdKey];
            if (tenantIdHeader == string.Empty || tenantIdHeader.Count < 1)
            {
                return Task.FromResult((string)null);
            }

            return Task.FromResult(tenantIdHeader.First());
        }
    }
}
