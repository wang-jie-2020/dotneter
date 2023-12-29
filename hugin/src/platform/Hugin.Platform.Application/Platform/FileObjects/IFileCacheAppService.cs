using System.Threading.Tasks;

namespace Hugin.Platform.FileObjects
{
    public interface IFileCacheAppService
    {
        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="file"></param>
        Task<string> SetFile(FileCto file);

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<FileCto> GetFile(string token);
    }
}
