using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    public class CookieStadiumResolveContributor : HttpStadiumResolveContributorBase
    {
        public const string ContributorName = "Cookie";

        public override string Name => ContributorName;

        protected override Task<string> GetStadiumIdOrNameFromHttpContextOrNullAsync(IStadiumResolveContext context, HttpContext httpContext)
        {
            return Task.FromResult(httpContext.Request.Cookies[context.GetAbpAspNetCoreMultiStadiumOptions().StadiumKey]);
        }
    }
}
