using JetBrains.Annotations;

namespace Yi.AspNetCore.MultiTenancy.Resolver;

public interface ITenantResolver
{
    /// <summary>
    /// Tries to resolve current tenant using registered <see cref="ITenantResolveContributor"/> implementations.
    /// </summary>
    /// <returns>
    /// Tenant id, unique name or null (if could not resolve).
    /// </returns>
    [NotNull]
    Task<TenantResolveResult> ResolveTenantIdOrNameAsync();
}
