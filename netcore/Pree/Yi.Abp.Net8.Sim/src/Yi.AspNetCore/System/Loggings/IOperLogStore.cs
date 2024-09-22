namespace Yi.AspNetCore.System.Loggings;

public interface IOperLogStore
{
    Task SaveAsync(OperLogInfo operLogInfo);
}