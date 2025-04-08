using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    public class FormStadiumResolveContributor : HttpStadiumResolveContributorBase
    {
        public const string ContributorName = "Form";

        public override string Name => ContributorName;

        protected override async Task<string> GetStadiumIdOrNameFromHttpContextOrNullAsync(IStadiumResolveContext context, HttpContext httpContext)
        {
            if (!httpContext.Request.HasFormContentType)
            {
                return null;
            }

            var form = await httpContext.Request.ReadFormAsync();
            return form[context.GetAbpAspNetCoreMultiStadiumOptions().StadiumKey];
        }
    }
}
