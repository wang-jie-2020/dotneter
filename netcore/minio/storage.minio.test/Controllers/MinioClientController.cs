using System.Diagnostics.Eventing.Reader;
using System.Reactive.Linq;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Tags;
using Minio.Exceptions;
using MinioStorage;
using Newtonsoft.Json;

namespace Demo.Controllers
{
    [ApiController]
    [Route("minio-client")]
    public class MinioClientController : ControllerBase
    {
        private readonly MinioClient _minioClient;
        private readonly string nonVersion = "non-version";
        private readonly string versioning = "versioning";
        private readonly string versioningLocking = "versioning-locking";
        private readonly string bucketName;

        public MinioClientController(IMinioContainer<TestContainer> testContainer)
        {
            _minioClient = testContainer.Client;
            bucketName = versioningLocking;
        }

        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
        /*
    
            -- key、eTag、versionId的概念问题
                key         object-name
                eTag        按key计算的一个uuid(只要名称一样eTag就一样)
                versionId   开启多版本的存储桶中保存的文件每一次上传的版本标识
                以 bucketName--objectName|eTag--versionId 唯一确定object

            -- prefix
                key的模糊查询

            -- tag
                按正常tag标志理解,但很奇怪的是CRUD操作未指定tag参数,按文档表述可以对存储桶和文件指定tag来绑定策略
                https://docs.aws.amazon.com/AmazonS3/latest/userguide/object-tagging.html
                https://docs.aws.amazon.com/AWSEC2/latest/UserGuide/Using_Tags.html#tag-restrictions
         
            -- SDK的API:Put的versionId、eTag,Get的eTag 不知道怎么用

            -- 多版本特性
                在不启动多版本时,正常CRUD理解
                在启动多版本时
                    PUT:                    同名不覆盖,产生不同的版本,api中version参数缺省时默认指向最新版本
                    LIST:                   总是可以显示未彻底删除的版本
                    GET:                    指向最新版本,若有删除标记,则不显示
                    GET+versionId:          只要未彻底删除,总是可以显示               
                    DELETE:                 在当前版本上增加删除标记,保留版本   
                    DELETE+version:         DELETE的基础上彻底删除(大小为0)
                在多版本的基础上才能locking,防止多版本时的彻底删除
                同时还有持续策略,暂不做继续查探
        /*	
        /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

        [HttpPost]
        [Route("client-post")]
        public async Task<object> Post(string name, IFormFile file)
        {
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(name)
                .WithStreamData(file.OpenReadStream())
                .WithObjectSize(file.OpenReadStream().Length));

            return await GetAsync(name);
        }

        [HttpGet]
        [Route("client-list")]
        public async Task<object> List(string? prefix = "", bool? withVersions = true)
        {
            var result = new List<Minio.DataModel.Item>();

            var items = _minioClient.ListObjectsAsync(new ListObjectsArgs()
                .WithBucket(bucketName)
                .WithVersions(withVersions ?? false)
                .WithPrefix(prefix)
                .WithRecursive(true));

            items.Subscribe(item => result.Add(item),
            ex => { },
                () => { });
            await items;

            return result;
        }

        [HttpGet]
        [Route("client-get")]
        public async Task<object> GetAsync(string name, string? version = "")
        {
            var memoryStream = new MemoryStream();

            var obj = await _minioClient.GetObjectAsync(new GetObjectArgs()
                 .WithBucket(bucketName)
                 .WithObject(name)
                 .WithVersionId(version)
                 .WithCallbackStream(stream =>
                 {
                     if (stream != null)
                     {
                         stream.CopyTo(memoryStream);
                         memoryStream.Seek(0, SeekOrigin.Begin);
                     }
                     else
                     {
                         memoryStream = null;
                     }
                 }));

            return obj;
        }

        [HttpDelete]
        [Route("client-delete")]
        public async Task DeleteAsync(string name, string? versionId)
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(name)
                .WithVersionId(versionId));
        }

        [HttpDelete]
        [Route("client-delete-with-passGovernance")]
        public async Task DeletePassGovernanceAsync(string name, string? versionId)
        {
            //绕过治理模式删除,还不能理解这个意思
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(name)
                .WithBypassGovernanceMode(true)
                .WithVersionId(versionId));
        }

        [HttpPost]
        [Route("client-post-with-tag")]
        public async Task<object> PostTag(string name, IFormFile file)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("tagA", "valueA");
            dic.Add("tagB", "valueB");

            var tag = new Tagging(dic, false);

            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(name)
                .WithTagging(tag)
                .WithStreamData(file.OpenReadStream())
                .WithObjectSize(file.OpenReadStream().Length));

            return await GetAsync(name);
        }

        //[HttpPost]
        //[Route("client-post-with-versionId")]
        //public async Task<object> PostVersionId(string name, string versionId, IFormFile file)
        //{
        //    await _minioClient.PutObjectAsync(new PutObjectArgs()
        //            .WithBucket(bucketName)
        //            .WithObject(name)
        //            .WithVersionId(versionId)
        //            .WithStreamData(file.OpenReadStream())
        //            .WithObjectSize(file.OpenReadStream().Length));

        //    return await GetAsync(name);
        //}

        //[HttpGet]
        //[Route("client-get-by-eTag")]
        //public async Task<object> GetByETagAsync(string name, string eTag, string? version = "")
        //{
        //    var memoryStream = new MemoryStream();

        //    var obj = await _minioClient.GetObjectAsync(new GetObjectArgs()
        //        .WithBucket(bucketName)
        //        .WithMatchETag(eTag)
        //        .WithVersionId(version)
        //        .WithCallbackStream(stream =>
        //        {
        //            if (stream != null)
        //            {
        //                stream.CopyTo(memoryStream);
        //                memoryStream.Seek(0, SeekOrigin.Begin);
        //            }
        //            else
        //            {
        //                memoryStream = null;
        //            }
        //        }));

        //    return obj;
        //}

        //[HttpGet]
        //[Route("client-get-by-not-eTag")]
        //public async Task<object> GetNotETagAsync(string name, string eTag, string? version = "")
        //{
        //    var memoryStream = new MemoryStream();

        //    var obj = await _minioClient.GetObjectAsync(new GetObjectArgs()
        //        .WithBucket(bucketName)
        //        .WithNotMatchETag(eTag)
        //        .WithVersionId(version)
        //        .WithCallbackStream(stream =>
        //        {
        //            if (stream != null)
        //            {
        //                stream.CopyTo(memoryStream);
        //                memoryStream.Seek(0, SeekOrigin.Begin);
        //            }
        //            else
        //            {
        //                memoryStream = null;
        //            }
        //        }));

        //    return obj;
        //}

        //[HttpPost]
        //[Route("client-post-with-eTag")]
        //public async Task<object> PostETag(string name, string eTag)
        //{
        //    await _minioClient.PutObjectAsync(new PutObjectArgs()
        //        .WithBucket(bucketName)
        //        .WithObject(name)
        //        .WithFileName(@"C:\Users\Administrator\Desktop\Demo\ver1.txt")
        //        .WithMatchETag(eTag));

        //    return 1;
        //}
    }
}