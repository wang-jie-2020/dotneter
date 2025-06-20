using JetBrains.Annotations;

namespace Yi.AspNetCore.MultiTenancy.Resolver;

public interface ITenantResolver
{
    [NotNull]
    Task<TenantResolveResult> ResolveTenantIdOrNameAsync();
}
