using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Yi.AspNetCore.System.Loggings;

public class SimpleOperLogStore : IOperLogStore
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