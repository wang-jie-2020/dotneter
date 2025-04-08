using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hugin.Oss
{
    public interface IOssService
    {
        /// <summary>
        /// 普通文件上传
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="container">目录名</param>
        /// <returns></returns>
        Task<OssFileInfo> Save(IFormFile file, string container);

        /// <summary>
        /// 分片上传
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="container">目录名</param>
        /// <param name="guid">唯一标识</param>
        /// <param name="chunk">片数</param>
        /// <returns></returns>
        Task ChunkSave(IFormFile file, string container, string guid, int chunk);

        /// <summary>
        /// 分片合并
        /// </summary>
        /// <param name="container">目录名</param>
        /// <param name="guid">唯一标识</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        Task<OssFileInfo> ChunkMerge(string container, string guid, string fileName);
    }
}
