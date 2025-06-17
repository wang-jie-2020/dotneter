using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Data;

namespace Volo.Abp.Data;

public class DefaultConnectionStringResolver : IConnectionStringResolver, ITransientDependency
{
    protected DbConnectionOptions Options { get; }

    public DefaultConnectionStringResolver(
        IOptionsMonitor<DbConnectionOptions> options)
    {
        Options = options.CurrentValue;
    }

    public virtual Task<string> ResolveAsync(string? connectionStringName = null)
    {
        return Task.FromResult(ResolveInternal(connectionStringName))!;
    }

    private string? ResolveInternal(string? connectionStringName)
    {
        if (connectionStringName == null)
        {
            return Options.ConnectionStrings.Default;
        }

        var connectionString = Options.GetConnectionStringOrNull(connectionStringName);

        if (!connectionString.IsNullOrEmpty())
        {
            return connectionString;
        }

        return null;
    }
}
