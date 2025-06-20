using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.MultiTenancy.Resolver;

public interface ITenantResolveContext : IServiceProviderAccessor
{
    string? TenantIdOrName { get; set; }

    bool Handled { get; set; }
}
