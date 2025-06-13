namespace Yi.AspNetCore.MultiTenancy.Resolver;

public interface ITenantResolveContributor
{
    string Name { get; }

    Task ResolveAsync(ITenantResolveContext context);
}
