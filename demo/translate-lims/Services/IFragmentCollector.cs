namespace Translate.Services
{
    public interface IFragmentCollector
    {
        bool Satisfied(FileInfo file);

        Task<CollectContext> CollectAsync(FileInfo file);
    }
}
