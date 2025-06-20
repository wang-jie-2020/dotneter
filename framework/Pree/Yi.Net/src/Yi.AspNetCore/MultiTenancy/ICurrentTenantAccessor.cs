namespace Yi.AspNetCore.MultiTenancy;

public interface ICurrentTenantAccessor
{
    TenantInfo? Current { get; set; }
}
