namespace Translate.Services
{
    public interface ICollectService
    {
        Task CollectFromFolderAsync(string folder, bool sink = false);

        Task CollectFormFileAsync(string path, bool sink = false);

        Task SinkAsync(CollectContext context);
    }
}
