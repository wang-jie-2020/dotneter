using Microsoft.AspNetCore.Http;

namespace Yi.AspNetCore.MultiTenancy.Resolver;

public class HeaderTenantResolveContributor : HttpTenantResolveContributorBase
{
    public const string ContributorName = "Header";

    public override string Name => ContributorName;

    protected override Task<string?> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
    {
        if (httpContext.Request.Headers.IsNullOrEmpty())
        {
            return Task.FromResult((string?)null);
        }

        var tenantIdKey = TenantResolverConsts.DefaultTenantKey;

        var tenantIdHeader = httpContext.Request.Headers[tenantIdKey];
        if (tenantIdHeader == string.Empty || tenantIdHeader.Count < 1)
        {
            return Task.FromResult((string?)null);
        }
        
        return Task.FromResult(tenantIdHeader.First());
    }
}
