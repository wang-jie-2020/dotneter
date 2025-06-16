using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Data;

public interface IConnectionStringResolver
{
    [NotNull]
    Task<string> ResolveAsync(string? connectionStringName = null);
}
