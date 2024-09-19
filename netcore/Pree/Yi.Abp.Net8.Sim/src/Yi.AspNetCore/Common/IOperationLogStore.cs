namespace Yi.AspNetCore.Common;

public interface IOperationLogStore
{
    Task SaveAsync(OperationLogInfo operationLogInfo);
}