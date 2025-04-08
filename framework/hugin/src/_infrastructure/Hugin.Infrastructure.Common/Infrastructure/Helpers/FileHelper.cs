using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Hugin.Infrastructure.Helpers
{
    public static class FileHelper
    {
        private static IHostingEnvironment _hostingEnvironment = new
            HttpContextAccessor().HttpContext.RequestServices.GetService
            (typeof(IHostingEnvironment)) as IHostingEnvironment;

        /// <summary>
        /// 目录分隔符
        /// windows "\" OSX and Linux  "/"
        /// </summary>
        private static readonly string DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();

        /// <summary>
        /// 包含应用程序的目录的绝对路径
        /// </summary>
        private static readonly string ContentRootPath = _hostingEnvironment.ContentRootPath;

        #region 检测指定路径是否存在

        /// <summary>
        /// 检测指定路径是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsExist(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return IsDirectory(MapPath(path)) ?
                Directory.Exists(MapPath(path)) :
                File.Exists(MapPath(path));
        }

        #endregion

        #region 检测目录是否为空

        /// <summary>
        /// 检测目录是否为空
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return Directory.GetFiles(MapPath(path)).Length <= 0
                   && Directory.GetDirectories(MapPath(path)).Length <= 0;
        }

        #endregion

        #region 创建目录

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">路径</param>
        public static void CreateFiles(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            try
            {
                if (IsDirectory(MapPath(path)))
                    Directory.CreateDirectory(MapPath(path));
                else
                    File.Create(MapPath(path)).Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除文件或目录

        /// <summary>
        /// 删除目录或文件
        /// </summary>
        /// <param name="path">路径</param>
        public static void DeleteFiles(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            try
            {
                if (IsExist(path))
                {
                    if (IsDirectory(MapPath(path)))
                        Directory.Delete(MapPath(path));
                    else
                        File.Delete(MapPath(path));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清空目录下所有文件及子目录，依然保留该目录
        /// </summary>
        /// <param name="path"></param>
        public static void ClearDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (IsExist(path))
            {
                //目录下所有文件
                string[] files = Directory.GetFiles(MapPath(path));
                foreach (var file in files)
                {
                    DeleteFiles(file);
                }
                //目录下所有子目录
                string[] directorys = Directory.GetDirectories(MapPath(path));
                foreach (var dir in directorys)
                {
                    DeleteFiles(dir);
                }
            }
        }

        #endregion

        #region 判断文件是否为隐藏文件（系统独占文件）

        /// <summary>
        /// 检测文件或文件夹是否为隐藏文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsHiddenFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return IsDirectory(MapPath(path)) ?
                InspectHiddenFile(
                    new DirectoryInfo(MapPath(path))) :
                InspectHiddenFile(new FileInfo(MapPath(path)));
        }

        /// <summary>
        /// 检测文件或文件夹是否为隐藏文件（异步方式）
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static async Task<bool> IsHiddenFileAsync(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return await Task.Run(() =>
                IsDirectory(MapPath(path)) ? InspectHiddenFile(new DirectoryInfo(MapPath(path))) :
                    InspectHiddenFile(new FileInfo(MapPath(path))));
        }

        /// <summary>
        /// 私有方法 文件是否为隐藏文件（系统独占文件）
        /// </summary>
        /// <param name="fileSystemInfo"></param>
        /// <returns></returns>
        private static bool InspectHiddenFile(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo == null) return false;
            if (fileSystemInfo.Name.StartsWith("."))
            {
                return true;
            }
            else if (fileSystemInfo.Exists &&
                     ((fileSystemInfo.Attributes & FileAttributes.Hidden) != 0 ||
                      (fileSystemInfo.Attributes & FileAttributes.System) != 0))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 文件操作

        #region 复制文件
        /// <summary>
        /// 复制文件内容到目标文件夹
        /// </summary>
        /// <param name="sourcePath">源文件</param>
        /// <param name="targetPath">目标文件夹</param>
        /// <param name="isOverWrite">是否可以覆盖</param>
        public static void Copy(string sourcePath, string targetPath, bool isOverWrite = true)
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath)) return;
            File.Copy(MapPath(sourcePath), MapPath(targetPath) +
                                           GetFileName(sourcePath), isOverWrite);
        }

        /// <summary>
        /// 复制文件内容到目标文件夹
        /// </summary>
        /// <param name="sourcePath">源文件</param>
        /// <param name="targetPath">目标文件夹</param>
        /// <param name="newName">新文件名称</param>
        /// <param name="isOverWrite">是否可以覆盖</param>
        public static void Copy(string sourcePath, string targetPath, string newName, bool isOverWrite = true)
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath)) return;
            File.Copy(MapPath(sourcePath), MapPath(targetPath) + newName, isOverWrite);
        }

        #endregion

        #region 移动文件

        /// <summary>
        /// 移动文件到目标目录
        /// </summary>
        /// <param name="sourcePath">源文件</param>
        /// <param name="targetPath">目标目录</param>
        public static void Move(string sourcePath, string targetPath)
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(targetPath)) return;
            string sourceFileName = GetFileName(sourcePath);
            //如果目标目录不存在则创建
            if (IsExist(targetPath))
            {
                CreateFiles(targetPath);
            }
            else
            {
                //如果目标目录存在同名文件则删除
                if (IsExist(Path.Combine(MapPath(targetPath), sourceFileName)))
                {
                    DeleteFiles(Path.Combine(MapPath(targetPath), sourceFileName));
                }
            }
            File.Move(MapPath(sourcePath), Path.Combine(MapPath(targetPath), sourcePath));
        }

        #endregion

        /// <summary>
        /// 获取文件名和扩展名
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return Path.GetFileName(MapPath(path));
        }

        /// <summary>
        /// 获取文件名不带扩展名
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetFileNameWithOutExtension(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return Path.GetFileNameWithoutExtension(MapPath(path));
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string GetFileExtension(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return Path.GetExtension(MapPath(path));
        }

        #endregion

        #region 获取文件绝对路径

        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return Path.Combine(ContentRootPath, path.TrimStart('~', '/').Replace("/", DirectorySeparatorChar));
        }

        /// <summary>
        /// 获取文件绝对路径（异步方式）
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static async Task<string> MapPathAsync(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return await Task.Run(() =>
                Path.Combine(ContentRootPath, path.TrimStart('~', '/').
                    Replace("/", DirectorySeparatorChar)));
        }

        /// <summary>
        /// 是否为目录或文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (path.EndsWith(DirectorySeparatorChar))
                return true;
            else
                return false;
        }

        #endregion

        #region 物理路径转虚拟路径

        public static string PhysicalToVirtual(string physicalPath)
        {
            if (string.IsNullOrEmpty(physicalPath)) return physicalPath;
            return physicalPath.Replace(ContentRootPath, "").Replace(DirectorySeparatorChar, "/");
        }

        #endregion

        #region 文件格式

        /// <summary>
        /// 是否可添加水印
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        public static bool IsCanWater(string fileExt)
        {
            if (string.IsNullOrEmpty(fileExt)) return false;
            var images = new List<string> { "jpg", "jpeg" };
            if (images.Contains(fileExt.ToLower())) return true;
            return false;
        }

        /// <summary>
        /// 是否为图片
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        public static bool IsImage(string fileExt)
        {
            if (string.IsNullOrEmpty(fileExt)) return false;
            var images = new List<string> { "bmp", "gif", "jpg", "jpeg", "png" };
            if (images.Contains(fileExt.ToLower())) return true;
            return false;
        }

        /// <summary>
        /// 是否为视频
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        public static bool IsVideos(string fileExt)
        {
            if (string.IsNullOrEmpty(fileExt)) return false;
            var videos = new List<string> { "rmvb", "mkv", "ts", "wma", "avi", "rm", "mp4", "flv", "mpeg", "mov", "3gp", "mpg" };
            if (videos.Contains(fileExt.ToLower())) return true;
            return false;
        }

        /// <summary>
        /// 是否为音频
        /// </summary>
        /// <param name="fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        public static bool IsMusics(string fileExt)
        {
            if (string.IsNullOrEmpty(fileExt)) return false;
            var musics = new List<string> { "mp3", "wav" };
            if (musics.Contains(fileExt.ToLower())) return true;
            return false;
        }

        /// <summary>
        /// 是否为文档
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <returns></returns>
        public static bool IsDocument(string fileExt)
        {
            if (string.IsNullOrEmpty(fileExt)) return false;
            var documents = new List<string> { "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "pdf" };
            if (documents.Contains(fileExt.ToLower())) return true;
            return false;
        }

        #endregion

        #region 文件图标

        public static string FindFileIcon(string fileExt)
        {
            if (string.IsNullOrEmpty(fileExt)) return fileExt;
            if (IsImage(fileExt))
                return "fa fa-image";
            if (IsVideos(fileExt))
                return "fa fa-film";
            if (IsMusics(fileExt))
                return "fa fa-music";
            if (IsDocument(fileExt))
                switch (fileExt.ToLower())
                {
                    case ".xls":
                    case ".xlsx":
                        return "fa fa-file-excel-o";
                    case ".ppt":
                    case ".pptx":
                        return "fa fa-file-powerpoint-o";
                    case ".pdf":
                        return "fa fa-file-pdf-o";
                    case ".txt":
                        return "fa fa-file-text-o";
                    default:
                        return "fa fa-file-word-o";
                }
            if (fileExt.ToLower() == "zip" || fileExt.ToLower() == "rar")
                return "fa fa-file-zip-o";
            else
                return "fa fa-file";
        }

        #endregion

        #region 文件大小转换

        /// <summary>
        ///  文件大小转为B、KB、MB、GB...
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string FileSizeTransfer(long size)
        {
            if (size == 0) return null;
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            long mod = 1024;
            int i = 0;
            while (size > mod)
            {
                size /= mod;
                i++;
            }
            return size + units[i];
        }

        #endregion

        #region 获取目录下所有文件

        public static List<FilesInfo> FindFiles(string path, string staticFiles = "/wwwroot")
        {
            if (string.IsNullOrEmpty(path)) return null;
            string[] folders = Directory.GetDirectories(MapPath(path), "*", SearchOption.AllDirectories);
            var files = new List<FilesInfo>();
            foreach (var folder in folders)
            {
                foreach (var fsi in new DirectoryInfo(folder).GetFiles())
                {
                    files.Add(new FilesInfo()
                    {
                        Name = fsi.Name,
                        FullName = fsi.FullName,
                        FileExt = fsi.Extension,
                        FileOriginalSize = fsi.Length,
                        FileSize = FileSizeTransfer(fsi.Length),
                        FileIcon = FindFileIcon(fsi.Extension.Remove(0, 1)),
                        FileName = PhysicalToVirtual(fsi.FullName).Replace(staticFiles, ""),
                        FileStyle = IsImage(fsi.Extension.Remove(0, 1)) ? "images" :
                            IsDocument(fsi.Extension.Remove(0, 1)) ? "documents" :
                            IsVideos(fsi.Extension.Remove(0, 1)) ? "videos" :
                            IsMusics(fsi.Extension.Remove(0, 1)) ? "musics" : "others",
                        CreateDate = fsi.CreationTime,
                        LastWriteDate = fsi.LastWriteTime,
                        LastAccessDate = fsi.LastAccessTime
                    });
                }
            }
            return files;
        }

        /// <summary>
        /// 获得指定文件夹下面的所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="staticFiles"></param>
        /// <returns></returns>
        public static List<FilesInfo> ResolveFileInfo(string path, string staticFiles = "/wwwroot")
        {
            if (string.IsNullOrEmpty(path)) return null;
            var foldersPath = MapPath(path);
            var Files = new List<FilesInfo>();
            foreach (var fsi in new DirectoryInfo(foldersPath).GetFiles())
            {
                Files.Add(new FilesInfo()
                {
                    Name = fsi.Name,
                    FullName = fsi.FullName,
                    FileExt = fsi.Extension,
                    FileOriginalSize = fsi.Length,
                    FileSize = FileSizeTransfer(fsi.Length),
                    FileIcon = FindFileIcon(fsi.Extension.Remove(0, 1)),
                    FileName = PhysicalToVirtual(fsi.FullName).Replace(staticFiles, ""),
                    FileStyle = IsImage(fsi.Extension.Remove(0, 1)) ? "images" :
                        IsDocument(fsi.Extension.Remove(0, 1)) ? "documents" :
                        IsVideos(fsi.Extension.Remove(0, 1)) ? "videos" :
                        IsMusics(fsi.Extension.Remove(0, 1)) ? "musics" : "others",
                    CreateDate = fsi.CreationTime,
                    LastWriteDate = fsi.LastWriteTime,
                    LastAccessDate = fsi.LastAccessTime
                });
            }
            return Files;
        }

        #endregion

        #region 检查文件格式是否未图片

        public static bool CheckFileType(string fileName)
        {
            var x = new List<string>() { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };

            var fileExtension = Path.GetExtension(fileName).ToLower();

            return x.Select(p => p.ToLower()).Contains(fileExtension);

        }

        #endregion
    }

    public class FilesInfo
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件物理路径
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string FileExt { get; set; }

        /// <summary>
        /// 原始大小（字节）
        /// </summary>
        public long FileOriginalSize { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// 文件虚拟路径
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileStyle { get; set; }

        /// <summary>
        /// 文件图标
        /// </summary>
        public string FileIcon { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastWriteDate { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessDate { get; set; }

    }
}