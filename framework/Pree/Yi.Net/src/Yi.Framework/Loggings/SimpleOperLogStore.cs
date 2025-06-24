using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Yi.Framework.Loggings;

[Dependency(TryRegister = true)]
public class SimpleOperLogStore : IOperLogStore, ISingletonDependency
{
    public ILogger<SimpleOperLogStore> Logger { get; set; }

    public SimpleOperLogStore()
    {
        Logger = NullLogger<SimpleOperLogStore>.Instance;
    }

    public Task SaveAsync(OperLogInfo operLogInfo)
    {
        Logger.LogInformation(operLogInfo.ToString());
        return Task.FromResult(0);
    }
}