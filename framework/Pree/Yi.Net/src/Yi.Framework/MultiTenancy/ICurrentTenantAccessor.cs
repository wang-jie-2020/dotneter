namespace Yi.AspNetCore.MultiTenancy;

public interface ICurrentTenantAccessor
{
    BasicTenantInfo? Current { get; set; }
}
