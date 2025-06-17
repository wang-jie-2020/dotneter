using JetBrains.Annotations;

namespace Yi.AspNetCore.Data;

public interface IConnectionStringResolver
{
    [NotNull]
    Task<string> ResolveAsync(string? connectionStringName = null);
}
