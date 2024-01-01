using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Minio;

namespace MinioStorage
{
    public interface IMinioContainer<TContainer> : IMinioContainer
        where TContainer : class
    {

    }

    public interface IMinioContainer
    {
        MinioClient Client { get; }

        Task<List<string>> ListAsync(string prefix = "");

        Task SaveAsync(string name, Stream stream, bool overrideExisting = false);

        Task<bool> ExistsAsync(string name);

        Task<Stream> GetAsync(string name);

        Task<bool> DeleteAsync(string name);

        Task<string> PresignedGetAsync(string name, int expiry);

        Task<string> PresignedSaveAsync(string name, int expiry, bool overrideExisting = false);
    }
}
