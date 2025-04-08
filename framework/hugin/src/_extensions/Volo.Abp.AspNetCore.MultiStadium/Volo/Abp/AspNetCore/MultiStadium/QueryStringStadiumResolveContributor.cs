using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    public class QueryStringStadiumResolveContributor : HttpStadiumResolveContributorBase
    {
        public const string ContributorName = "QueryString";

        public override string Name => ContributorName;

        protected override Task<string> GetStadiumIdOrNameFromHttpContextOrNullAsync(IStadiumResolveContext context, HttpContext httpContext)
        {
            return Task.FromResult(httpContext.Request.QueryString.HasValue
                ? httpContext.Request.Query[context.GetAbpAspNetCoreMultiStadiumOptions().StadiumKey].ToString()
                : null);
        }
    }
}
