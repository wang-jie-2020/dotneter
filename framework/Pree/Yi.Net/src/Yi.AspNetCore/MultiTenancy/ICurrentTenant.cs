namespace Yi.AspNetCore.MultiTenancy;

public interface ICurrentTenant
{
    long? Id { get; }

    string? Name { get; }

    IDisposable Change(long? id, string? name = null);
}
