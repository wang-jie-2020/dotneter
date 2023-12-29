using System;

namespace Hugin.Oss
{
    public class OssFileInfo
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 文件md5
        /// </summary>
        public string ContentMd5 { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? LastModified { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 目录名
        /// </summary>
        public string Container { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
    }
}
