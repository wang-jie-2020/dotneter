using JetBrains.Annotations;

namespace Yi.AspNetCore.MultiTenancy.Resolver;

public class TenantResolveOptions
{
    [NotNull]
    public List<ITenantResolveContributor> TenantResolvers { get; }

    public TenantResolveOptions()
    {
        TenantResolvers =
        [
            new HeaderTenantResolveContributor(),
            new RouteTenantResolveContributor()
        ];
    }
}
