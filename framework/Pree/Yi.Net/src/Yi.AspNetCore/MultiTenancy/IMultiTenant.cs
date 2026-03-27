namespace Yi.AspNetCore.MultiTenancy;

public interface IMultiTenant
{
    long? TenantId { get; }
}
