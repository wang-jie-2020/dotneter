using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using Yi.Infra.Rbac.Dtos.FileManager;
using Yi.Infra.Rbac.IServices;

namespace Yi.Infra.Rbac.Services;

public class FileService : ApplicationService, IFileService
{
    /// <summary>
    ///     上传文件
    ///     Todo: 可放入领域层
    /// </summary>
    /// <returns></returns>
    public async Task<List<FileGetListOutputDto>> Post([FromForm] IFormFileCollection file)
    {
        throw new NotImplementedException();
    }
}