using Microsoft.AspNetCore.Mvc;
using Utils.Minio;
using Volo.Abp.AspNetCore.Mvc;
using Yitter.IdGenerator;

namespace Yi.Web.Controllers.System;

[ApiController]
[Route("api/system/file")]
public class FileController : AbpController
{
    private readonly IMinioContainer<DefaultContainer> _container;

    public FileController(IMinioContainer<DefaultContainer> container)
    {
        _container = container;
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<string>> Post([FromForm] IFormFileCollection file)
    {
        if (file.Count == 0)
        {
            throw new ArgumentException("missing files");
        }

        var urls = new List<string>();
        foreach (var f in file)
        {
            var name = Path.GetFileNameWithoutExtension(f.FileName) + YitIdHelper.NextId() + Path.GetExtension(f.FileName);
            var stream = f.OpenReadStream();
            
            var url = await _container.PublishAsync(name, stream);
            urls.Add(url);
        }

        return urls;
    }

    [ContainerName("default")]
    public class DefaultContainer
    {
    }
}