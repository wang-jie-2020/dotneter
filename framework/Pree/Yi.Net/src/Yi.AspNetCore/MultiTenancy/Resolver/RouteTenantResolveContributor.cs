using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Yi.AspNetCore.MultiTenancy.Resolver;

public class RouteTenantResolveContributor : HttpTenantResolveContributorBase
{
    public const string ContributorName = "Route";

    public override string Name => ContributorName;

    protected override Task<string?> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
    {
        var tenantId = httpContext.GetRouteValue(TenantResolverConsts.DefaultTenantKey);
        return Task.FromResult(tenantId != null ? Convert.ToString(tenantId) : null);
    }
}
