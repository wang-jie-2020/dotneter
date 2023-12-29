using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.MultiStadium;

namespace Volo.Abp.AspNetCore.MultiStadium
{
    public abstract class HttpStadiumResolveContributorBase : StadiumResolveContributorBase
    {
        public override async Task ResolveAsync(IStadiumResolveContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext == null)
            {
                return;
            }

            try
            {
                await ResolveFromHttpContextAsync(context, httpContext);
            }
            catch (Exception e)
            {
                context.ServiceProvider
                    .GetRequiredService<ILogger<HttpStadiumResolveContributorBase>>()
                    .LogWarning(e.ToString());
            }
        }

        protected virtual async Task ResolveFromHttpContextAsync(IStadiumResolveContext context, HttpContext httpContext)
        {
            var tenantIdOrName = await GetStadiumIdOrNameFromHttpContextOrNullAsync(context, httpContext);
            if (!tenantIdOrName.IsNullOrEmpty())
            {
                context.StadiumIdOrName = tenantIdOrName;
            }
        }

        protected abstract Task<string> GetStadiumIdOrNameFromHttpContextOrNullAsync([NotNull] IStadiumResolveContext context, [NotNull] HttpContext httpContext);
    }
}
