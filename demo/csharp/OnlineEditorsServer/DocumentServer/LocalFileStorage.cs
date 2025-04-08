using Volo.Abp.DependencyInjection;

namespace OnlineEditorsServer.DocumentServer;

public class LocalFileStorage : IFileStorage, ITransientDependency
{
    private string baseDir;
    private string hisName = "-hist";

    public LocalFileStorage(IHostEnvironment environment)
    {
        baseDir = environment.ContentRootPath;
    }

    public async Task<int> GetMaxHisVersionAsync(string name)
    {
        var hisDir = Path.Combine(baseDir, name + hisName);
        return !Directory.Exists(hisDir) ? 0 : Directory.EnumerateDirectories(hisDir).Count();
    }

    public Task SaveAsync(string name, byte[] data)
    {
        var filePath = Path.Combine(baseDir, name);

        using (var fs = File.Open(filePath, FileMode.OpenOrCreate))
        {
            fs.Write(data, 0, data.Length);
        }

        return Task.CompletedTask;
    }

    public Task ForceSaveAsync(string name, byte[] data)
    {
        var hisDir = Path.Combine(baseDir, name + hisName);
        EnsureDirExists(hisDir);

        var fileName = $"{DateTime.Now:yyyyMMddHHmmss}_" + name;
        var filePath = Path.Combine(hisDir, fileName);
        
        using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            stream.Write(data, 0, data.Length);
        }

        return Task.CompletedTask;
    }

    public Task AsHisAsync(string name, int version)
    {
        var hisDir = Path.Combine(baseDir, name + hisName, version.ToString());
        EnsureDirExists(hisDir);

        File.Move(Path.Combine(baseDir, name), Path.Combine(hisDir, name));

        return Task.CompletedTask;
    }

    public Task HisRecordAsync(string name, int version, string recordName, byte[] recordData)
    {
        var hisDir = Path.Combine(baseDir, name + hisName, version.ToString());
        EnsureDirExists(hisDir);
        
        var recordPath = Path.Combine(hisDir, recordName);
        using (var stream = new FileStream(recordPath, FileMode.OpenOrCreate))
        {
            stream.Write(recordData, 0, recordData.Length);
        }

        return Task.CompletedTask;
    }

    private void EnsureDirExists(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
}