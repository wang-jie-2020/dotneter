using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.Mvc.OperLogging;

[Dependency(TryRegister = true)]
public class NullOperLogStore : IOperLogStore, ISingletonDependency
{
    public ILogger<NullOperLogStore> Logger { get; set; }

    public NullOperLogStore()
    {
        Logger = NullLogger<NullOperLogStore>.Instance;
    }

    public Task SaveAsync(OperLogInfo operLogInfo)
    {
        Logger.LogInformation(operLogInfo.ToString());
        return Task.FromResult(0);
    }
}