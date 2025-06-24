namespace Yi.AspNetCore.MultiTenancy;

public interface IMultiTenant
{
    Guid? TenantId { get; }
}
