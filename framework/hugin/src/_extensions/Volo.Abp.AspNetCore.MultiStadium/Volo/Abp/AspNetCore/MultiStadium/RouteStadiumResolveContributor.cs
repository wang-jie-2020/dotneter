using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    public class RouteStadiumResolveContributor : HttpStadiumResolveContributorBase
    {
        public const string ContributorName = "Route";

        public override string Name => ContributorName;

        protected override Task<string> GetStadiumIdOrNameFromHttpContextOrNullAsync(IStadiumResolveContext context, HttpContext httpContext)
        {
            var tenantId = httpContext.GetRouteValue(context.GetAbpAspNetCoreMultiStadiumOptions().StadiumKey);
            return Task.FromResult(tenantId != null ? Convert.ToString(tenantId) : null);
        }
    }
}
