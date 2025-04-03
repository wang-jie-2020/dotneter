namespace OnlineEditorsServer.DocumentServer;

public interface IFileStorage
{
    Task<int> GetMaxHisVersionAsync(string name);

    Task SaveAsync(string name, byte[] data);

    Task ForceSaveAsync(string name, byte[] data);

    Task AsHisAsync(string name, int version);
    
    Task HisRecordAsync(string name, int version, string recordName, byte[] recordData);
}