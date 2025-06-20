namespace Yi.Framework.Loggings;

public interface IOperLogStore
{
    Task SaveAsync(OperLogInfo operLogInfo);
}