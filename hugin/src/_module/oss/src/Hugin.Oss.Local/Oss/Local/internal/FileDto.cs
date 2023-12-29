namespace Hugin.Oss.Local.@internal
{
    internal class FileDto
    {
        public bool Success { get; set; } = true;

        public string Id { get; set; }

        /// <summary>
        /// 绝对Url，域名或IP部署时可用
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 相对Url，转发部署时可用
        /// </summary>
        public string RelativeUrl { get; set; }

        public FileRecord File { get; set; }
    }
}
