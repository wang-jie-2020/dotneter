using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Yi.AspNetCore.Common;

public class SimpleOperationLogStore : IOperationLogStore
{
    public ILogger<SimpleOperationLogStore> Logger { get; set; }

    public SimpleOperationLogStore()
    {
        Logger = NullLogger<SimpleOperationLogStore>.Instance;
    }

    public Task SaveAsync(OperationLogInfo operationLogInfo)
    {
        Logger.LogInformation(operationLogInfo.ToString());
        return Task.FromResult(0);
    }
}