namespace OnlineEditorsServer.DocumentServer;

public interface ICallbackManager
{
    Task<Dictionary<string, object>> ProcessCommandAsync(string method, string key, string host, object meta = null);

    Task<int> ProcessSaveAsync(string name, Callback callback);

    Task<int> ProcessForceSaveAsync(string name, Callback callback);
}