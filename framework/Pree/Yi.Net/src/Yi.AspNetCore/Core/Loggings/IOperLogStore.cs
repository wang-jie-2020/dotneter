namespace Yi.AspNetCore.Core.Loggings;

public interface IOperLogStore
{
    Task SaveAsync(OperLogInfo operLogInfo);
}