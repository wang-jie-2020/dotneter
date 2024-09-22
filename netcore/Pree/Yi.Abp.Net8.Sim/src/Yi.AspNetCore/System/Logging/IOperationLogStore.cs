namespace Yi.AspNetCore.System.Logging;

public interface IOperationLogStore
{
    Task SaveAsync(OperationLogInfo operationLogInfo);
}