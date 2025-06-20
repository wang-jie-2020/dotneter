namespace Yi.AspNetCore;

public interface IMultiTenant
{
    Guid? TenantId { get; }
}
