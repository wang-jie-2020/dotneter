namespace Yi.AspNetCore.MultiTenancy;

public interface ICurrentTenant
{
    Guid? Id { get; }

    string? Name { get; }

    IDisposable Change(Guid? id, string? name = null);
}
