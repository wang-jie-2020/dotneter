namespace Yi.AspNetCore.Mvc.OperLogging;

public interface IOperLogStore
{
    Task SaveAsync(OperLogInfo operLogInfo);
}