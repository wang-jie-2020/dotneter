using System.Text;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;

namespace OnlineEditorsServer.DocumentServer;

public class DefaultCallbackManager : ICallbackManager, ITransientDependency
{
    private readonly ILogger<DefaultCallbackManager> _logger;
    private readonly IFileStorage _fileStorage;

    public DefaultCallbackManager(
        ILogger<DefaultCallbackManager> logger,
        IFileStorage fileStorage)
    {
        _logger = logger;
        _fileStorage = fileStorage;
    }

    public async Task<Dictionary<string, object>> ProcessCommandAsync(string method, string key, string host, object meta = null)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(host);

            var body = new Dictionary<string, object>
            {
                { "c", method },
                { "key", key },
            };

            if (meta != null)
            {
                body.Add("meta", meta);
            }

            var response = await httpClient.PostAsync("/command", new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));
            var dataString = await response.Content.ReadAsStringAsync();
            var dataObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataString);
            if (dataObj["error"] == null || dataObj["error"].ToString()!.Equals("0"))
            {
                throw new Exception(dataString);
            }

            return dataObj;
        }
    }

    public async Task<int> ProcessSaveAsync(string name, Callback callback)
    {
        if (string.IsNullOrWhiteSpace(callback.Url))
        {
            throw new Exception("Url is required");
        }

        try
        {
            var bytes = await DownloadAsync(callback.Url);

            // move to his
            var v = await _fileStorage.GetMaxHisVersionAsync(name) + 1;
            await _fileStorage.AsHisAsync(name, v);

            // save current new
            await _fileStorage.SaveAsync(name, bytes);

            // save current changes
            byte[] bytesChanges = await DownloadAsync(callback.ChangesUrl);
            await _fileStorage.HisRecordAsync(name, v, "diff.zip", bytesChanges);

            if (callback.History != null)
            {
                var his = JsonConvert.SerializeObject(callback.History);
                await _fileStorage.HisRecordAsync(name, v, "changes.json", Encoding.UTF8.GetBytes(his));
            }

            await _fileStorage.HisRecordAsync(name, v, "key.txt", Encoding.UTF8.GetBytes(callback.Key));
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return 1;
        }

        return 0;
    }

    public async Task<int> ProcessForceSaveAsync(string name, Callback callback)
    {
        if (string.IsNullOrWhiteSpace(callback.Url))
        {
            throw new Exception("Url is required");
        }

        try
        {
            var bytes = await DownloadAsync(callback.Url);
            await _fileStorage.ForceSaveAsync(name, bytes);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return 1;
        }

        return 0;
    }

    private async Task<byte[]> DownloadAsync(string url)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(url);
            await using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await responseStream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}