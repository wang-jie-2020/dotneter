﻿using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Yi.Framework.Core.Enums;
using Yi.Framework.Core.Helper;
using Yi.Infra.Rbac.Dtos.FileManager;
using Yi.Infra.Rbac.Entities;
using Yi.Infra.Rbac.IServices;

namespace Yi.Infra.Rbac.Services;

public class FileService : ApplicationService, IFileService
{
    private readonly IRepository<FileAggregateRoot> _repository;
    private readonly IGuidGenerator _guidGenerator;

    public FileService(IRepository<FileAggregateRoot> repository, IGuidGenerator guidGenerator)
    {
        _guidGenerator = guidGenerator;
        _repository = repository;
    }

    public async Task<string> GetReturnPathAsync(Guid code, bool? isThumbnail)
    {
        var file = await _repository.GetAsync(x => x.Id == code);
        if (file is null) throw new UserFriendlyException("文件编号未匹配", "404");
        var path = file.FilePath;
        //如果为缩略图，需要修改路径
        //if (isThumbnail is true)
        //{
        //    path = $"wwwroot/{FileTypeEnum.Thumbnail}/{file.Id}{Path.GetExtension(file.FileName)}";
        //}
        //路径为： 文件路径/文件id+文件扩展名

        if (!File.Exists(path)) throw new UserFriendlyException("本地文件不存在", "404");

        return path;
    }

    /// <summary>
    ///     上传文件
    ///     Todo: 可放入领域层
    /// </summary>
    /// <returns></returns>
    public async Task<List<FileGetListOutputDto>> Post([FromForm] IFormFileCollection file)
    {
        if (file.Count() == 0) throw new ArgumentException("文件上传为空！");
        //批量插入
        List<FileAggregateRoot> entities = new();

        foreach (var f in file)
        {
            FileAggregateRoot data = new(_guidGenerator.Create());
            data.FileSize = (decimal)f.Length / 1024;
            data.FileName = f.FileName;

            var type = MimeHelper.GetFileType(f.FileName);

            //落盘文件，文件名为雪花id+自己的扩展名
            var filename = data.Id + Path.GetExtension(f.FileName);
            var typePath = $"wwwroot/{type}";
            if (!Directory.Exists(typePath)) Directory.CreateDirectory(typePath);
            var filePath = Path.Combine(typePath, filename);
            data.FilePath = filePath;


            //生成文件
            using (var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                await f.CopyToAsync(stream);

                //如果是图片类型，还需要生成缩略图,当然，如果图片很小，直接复制过去即可
                if (FileTypeEnum.Image.Equals(type))
                {
                    var thumbnailPath = $"wwwroot/{FileTypeEnum.Thumbnail}";
                    if (!Directory.Exists(thumbnailPath)) Directory.CreateDirectory(thumbnailPath);
                    var thumbnailFilePath = Path.Combine(thumbnailPath, filename);
                    try
                    {
                        // _imageSharpManager.ImageCompress(f.FileName, f.OpenReadStream(), thumbnailFilePath);
                    }
                    catch
                    {
                        var result = new byte[stream.Length];
                        await stream.ReadAsync(result, 0, result.Length);
                        await File.WriteAllBytesAsync(thumbnailFilePath, result);
                    }
                }
            }

            ;
            entities.Add(data);
        }

        await _repository.InsertManyAsync(entities);
        return entities.Adapt<List<FileGetListOutputDto>>();
    }

    /// <summary>
    ///     下载文件,是否缩略图
    /// </summary>
    /// <returns></returns>
    [Route("file/{code}/{isThumbnail?}")]
    public async Task<IActionResult> Get([FromRoute] Guid code, [FromRoute] bool? isThumbnail)
    {
        var path = await GetReturnPathAsync(code, isThumbnail);

        if (!File.Exists(path)) throw new UserFriendlyException("文件不存在", "404");


        var steam = await File.ReadAllBytesAsync(path);

        //考虑从路径中获取
        var fileContentType = MimeHelper.GetMimeMapping(Path.GetFileName(path));
        //设置附件下载，下载名称
        //_httpContext.FileAttachmentHandle(file.FileName);
        return new FileContentResult(steam, fileContentType ?? @"text/plain");
    }
}