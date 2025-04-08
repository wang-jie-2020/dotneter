using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Hugin.Oss.Aliyun
{
    public class AliyunOssService : IOssService
    {
        private readonly AliyunOssOptions _options;
        private readonly string _baseUrl;
        private readonly OssClient _ossClient;

        public string ProviderName => "AliyunOss";

        public AliyunOssService(IOptions<AliyunOssOptions> options)
        {
            _options = options.Value;
            _ossClient = new OssClient(_options.Endpoint, _options.AccessKeyId, _options.AccessKeySecret);
            _baseUrl = $"https://{_options.BucketName}.{_options.Endpoint}";
        }

        #region Copy的代码

        /// <summary>
        /// 保存对象到指定的容器
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        public async Task Save(string containerName, string fileName, Stream stream)
        {
            await Task.Run(() =>
                {
                    var key = $"{containerName}/{fileName}";
                    var md5 = OssUtils.ComputeContentMd5(stream, stream.Length);
                    var objectMeta = new ObjectMetadata();
                    objectMeta.AddHeader("Content-MD5", md5);
                    objectMeta.UserMetadata.Add("Content-MD5", md5);
                    _ossClient.PutObject(_options.BucketName, key, stream, objectMeta).HandlerError("上传对象出错");
                });
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<Stream> Get(string containerName, string fileName)
        {

            return await Task.Run(() =>
            {
                var key = $"{containerName}/{fileName}";
                var blob = _ossClient.GetObject(_options.BucketName, key).HandlerError("获取对象出错");
                if (blob == null || blob.ContentLength == 0)
                {
                    throw new Exception("没有找到该文件");
                }
                return blob.Content;
            });
        }

        /// <summary>
        /// 获取文件链接
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> GetUrl(string containerName, string fileName)
        {
            return await Task.Run(() => $"{_baseUrl}/{containerName}/{fileName}");
        }

        /// <summary>
        /// 获取对象属性
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<OssFileInfo> GetFileInfo(string containerName, string fileName)
        {
            return await Task.Run(() =>
            {
                var key = $"{containerName}/{fileName}";
                var result = _ossClient.GetObjectMetadata(_options.BucketName, key);
                return new OssFileInfo
                {
                    Container = containerName,
                    ETag = result.ETag,
                    LastModified = result.LastModified,
                    Name = fileName,
                    Length = result.ContentLength,
                    Url = $"{_baseUrl}/{containerName}/{fileName}",
                    ContentMd5 = result.ContentMd5,
                    ContentType = result.ContentType
                };
            });
        }

        /// <summary>
        /// 列出指定容器下的对象列表
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public async Task<IList<OssFileInfo>> ListFiles(string containerName)
        {
            var ossFileInfos = new List<OssFileInfo>();

            return await Task.Run(() =>
            {
                if (!string.IsNullOrWhiteSpace(containerName) && !containerName.EndsWith("/"))
                {
                    containerName += "/";
                }
                var listObjectsRequest = new ListObjectsRequest(_options.BucketName)
                {
                    Prefix = containerName
                };
                var result = _ossClient.ListObjects(listObjectsRequest).HandlerError("获取对象列表出错！");
                foreach (var summary in result.ObjectSummaries)
                {
                    ossFileInfos.Add(new OssFileInfo
                    {
                        Container = summary.BucketName,
                        ETag = summary.ETag,
                        LastModified = summary.LastModified,
                        Name = summary.Key,
                        Length = summary.Size,
                        Url = $"{_baseUrl}/{summary.BucketName}/{summary.Key}"
                    });
                }

                return ossFileInfos;
            });
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="fileName"></param>
        public async Task Delete(string containerName, string fileName)
        {
            await Task.Run(() =>
                {
                    var key = $"{containerName}/{fileName}";
                    _ossClient.DeleteObject(_options.BucketName, key);

                });
        }

        /// <summary>
        /// 删除目录（会删除下面所有的文件）
        /// </summary>
        /// <param name="containerName"></param>
        public async Task DeleteContainer(string containerName)
        {
            //删除目录等于删除该目录下的所有文件
            var files = await ListFiles(containerName);
            await Task.Run(() =>
            {
                var count = files.Count / 1000 + (files.Count % 1000 > 0 ? 1 : 0);

                for (var i = 0; i < count; i++)
                {
                    var request = new DeleteObjectsRequest(_options.BucketName,
                        files.Skip(i * 1000).Take(1000).Select(p => $"{containerName}/{p.Name}").ToList());

                    _ossClient.DeleteObjects(request).HandlerError("删除对象时出错(删除目录会删除该目录下所有的文件)!");
                }
            });
        }

        #endregion

        public async Task<OssFileInfo> Save(IFormFile file, string container)
        {
            await Save(container, file.FileName, file.OpenReadStream());
            return await GetFileInfo(container, file.FileName);
        }

        public Task ChunkSave(IFormFile file, string container, string guid, int chunk)
        {
            throw new NotImplementedException();
        }

        public Task<OssFileInfo> ChunkMerge(string container, string guid, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
