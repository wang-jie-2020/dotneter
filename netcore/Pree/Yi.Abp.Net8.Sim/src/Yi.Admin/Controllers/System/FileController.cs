using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utils.Minio;
using Volo.Abp.AspNetCore.Mvc;

namespace Yi.Admin.Controllers.System;

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
    public async Task<List<string>> Post([FromForm] IFormFileCollection fileCollection)
    {
        if (fileCollection.Count() == 0)
        {
            throw new ArgumentException("文件上传为空！");
        }

        var urls = new List<string>();
        foreach (var file in fileCollection)
        {
            var url = await _container.PublishAsync(file.FileName, file.OpenReadStream(), false);
            urls.Add(url);
        }

        return urls;
    }
    
    [ContainerName("default")]
    public class  DefaultContainer
    {
        
    }
}